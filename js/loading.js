// Advent of Code Loading Screen Handler
(function () {
  'use strict';

  let loadingHidden = false;

  // Function to hide the loading screen
  function hideLoadingScreen() {
    if (loadingHidden) return;
    loadingHidden = true;

    const loadingScreen = document.getElementById('advent-loading-screen');
    if (loadingScreen) {
      loadingScreen.classList.add('fade-out');
      setTimeout(() => {
        loadingScreen.style.display = 'none';
      }, 500); // Match the CSS transition duration
    }
  }

  // Function to check if Blazor app is ready
  function checkBlazorReady() {
    // Check for the presence of Blazor-specific elements
    const blazorScript = document.querySelector('script[src*="blazor.webassembly.js"]');
    const appElement = document.getElementById('app');

    if (appElement && appElement.children.length > 1) {
      // App has loaded content beyond the loading screen
      hideLoadingScreen();
      return true;
    }
    return false;
  }

  // Polling function to check for app readiness
  function pollForAppReady() {
    if (checkBlazorReady()) return;

    // Use MutationObserver to watch for changes in the app element
    const appElement = document.getElementById('app');
    if (appElement) {
      const observer = new MutationObserver(function (mutations) {
        mutations.forEach(function (mutation) {
          if (mutation.type === 'childList' &&
            appElement.children.length > 1) {
            observer.disconnect();
            setTimeout(hideLoadingScreen, 300); // Small delay for smooth transition
          }
        });
      });

      observer.observe(appElement, {
        childList: true,
        subtree: true
      });
    }

    // Fallback polling
    const pollInterval = setInterval(() => {
      if (checkBlazorReady()) {
        clearInterval(pollInterval);
      }
    }, 100);

    // Maximum timeout to prevent infinite loading
    setTimeout(() => {
      clearInterval(pollInterval);
      hideLoadingScreen();
    }, 15000); // 15 seconds max
  }

  // Initialize when DOM is ready
  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', pollForAppReady);
  } else {
    pollForAppReady();
  }

  // Additional safety net for window load
  window.addEventListener('load', function () {
    setTimeout(() => {
      if (!loadingHidden) {
        hideLoadingScreen();
      }
    }, 2000);
  });
})();
