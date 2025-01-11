# WebPulse




# Project Idea

WebPulse is a local app built using WPF and .NET, designed to keep users updated on content like TV series episodes, e-books, mangas, and similar types of media. The app offers a flexible approach to monitoring content across a wide range of websites, including small, unknown, and even less reliable sites. This is made possible by allowing users to set up monitoring tasks in ways that best fit their needs.

When setting up a monitoring task, users can choose between two options:

1. Provide a URL and specify the variable part of the URL (e.g., the dynamic episode number).
2. Supply JavaScript code that WebPulse can run to change the page, such as triggering a "Next" button to navigate.

At its core, WebPulse uses the Puppeteer library to perform web scraping, allowing it to fetch and track content updates from the specified websites. This approach ensures that users can monitor a wide variety of online content with minimal effort, regardless of how the website is structured.



# Project Roadmap

<!-- GUI part of the WebPulse project, covering front-end features and instructions -->

## 1. Home Section
   - **New Release**: View and manage the latest content released.
   - **Total New Releases**: See the overall number of new releases tracked by the app.
   - **Watching**: Monitor content that you are currently tracking for updates.

## 2. Monitoring
   - **Add New Monitoring**: Instructions on adding new items to monitor.
   - **All Monitoring Lists**: View a complete list of all the content being monitored.
   - **Guide to Add Monitoring**: Step-by-step guide on how to add a new monitoring entry.

## 3. Help
   - **Website Guide to Add Monitoring**:
     - **Method 1**: Instructions on using Method 1 for adding new monitoring.
     - **Method 2**: Instructions on using Method 2 for adding new monitoring.

## 4. Settings
   - **Some Settings**: Overview of configurable settings for personalizing the app.

<!-- Backend stuff of the WebPulse project, covering the back-end functionality -->

## 5. Navigation
   - **Navigation**: Manage and optimize the app's navigation structure and flow.

## 6. Monitoring Logic Using Puppeteer
   - **Monitoring Logic**: Implement the core logic for monitoring websites using Puppeteer to scrape and track updates.

## 7. Data Integration into GUI
   - **Data Integration**: Seamlessly integrate the backend data into the GUI for real-time updates and user interaction.

## 8. Notifications
   - **Notifications**: Implement notifications to alert users about new content, updates, or changes to the monitored items.
