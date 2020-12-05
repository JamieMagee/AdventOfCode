
// keep a handle on the original browser console.error
const mmerr = console.error;
const xhrsend = window.XMLHttpRequest.prototype.send;
const maxResourceCount = 41;
const logResourceCount = true;
let loadedResourceCount = 0;

// hook up our custom error function and mono monitor
function myload() {
    console.error = showError;
    window.XMLHttpRequest.prototype.send = newXhrSend;
    const totalProgress = document.querySelector('#totalProgress');
    totalProgress.max = maxResourceCount;
}

function newXhrSend(data) {
    this.addEventListener("progress", monitorXHR);
    this.addEventListener("loadend", endXHR);
    return xhrsend.apply(this, arguments);
}

// custom error function so we can display to the user
function showError(e) {
    // the "file_list" will be removed by Blazor if it runs
    // so if this is missing we can stop monitoring errors

    if (document.querySelector('#file_list')) {

        // show the error message div
        let errdiv = document.querySelector("#errordiv");
        errdiv.classList.remove("d-none");

        // add the error to the list
        let errdesc = document.querySelector("#errordescription");
        errdesc.innerHTML += '<li class="list-group-item alert-danger">' + e + '</li>';

    } else {
        // call the original error function and reset it - our app should be running
        mmerr(e);
        console.error = mmerr;
    }
}

function monitorXHR(e) {
    try {
        const fileList = document.querySelector('#file_list');
        if (!fileList) { return; }

        // We only need to monitor files if the file_list is there
        const url = e.currentTarget.responseURL;
        const filename = url.substring(url.lastIndexOf('/') + 1);
        const pr = fileList.querySelector("li[name='" + filename + "'] > progress");
        if (pr) {
            pr.value = e.loaded;
            return;
        }

        const item = document.createElement("li");
        item.setAttribute("name", filename);
        item.classList.add("d-flex", "w-100", "p-1");
        let previous = localStorage.getItem(filename);
        if (!previous) {
            previous = 4450000;
        }
        item.innerHTML = "" +
            "<progress width='100' style='margin-right: 5px; margin-top: 3px; margin-left: 17px' min='0' max='" + previous + "' value='" + e.loaded + "'></progress>" +
            "<span> " + filename + " </span>";

        fileList.appendChild(item);
    } catch (e) {
        console.error(e);
    }
}

function endXHR(e) {
    const url = e.currentTarget.responseURL;
    const filename = url.substring(url.lastIndexOf('/') + 1);
    localStorage.setItem(filename, e.loaded);

    // Update total progress
    loadedResourceCount++;
    if (logResourceCount) {
        console.info("Loaded resource #" + loadedResourceCount + ": " + filename);
    }
    const totalProgress = document.querySelector('#totalProgress');
    if (totalProgress) {
        totalProgress.value = Math.min(maxResourceCount - 1, loadedResourceCount);
    }

    const fileList = document.querySelector('#file_list');
    if (fileList) {
        // Update progress bars which did not know their max value
        const pr = fileList.querySelector("li[name='" + filename + "'] > progress");
        if (pr) { pr.max = e.loaded; pr.value = e.loaded; }

        // Remove completed items from the list
        const prs = fileList.querySelectorAll("li > progress");
        prs.forEach((pr) => {
            if (fileList.querySelectorAll("li > progress").length <= 1) {
                return false;
            }
            if (pr.value >= pr.max) {
                pr.parentNode.parentNode.removeChild(pr.parentNode);
            }
        });
    }
}
