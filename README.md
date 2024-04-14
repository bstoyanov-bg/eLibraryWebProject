
![GitHub repo size](https://img.shields.io/github/repo-size/bstoyanov-bg/eLibraryWebProject) 
![GitHub language count](https://img.shields.io/github/languages/count/bstoyanov-bg/eLibraryWebProject) 
![GitHub top language](https://img.shields.io/github/languages/top/bstoyanov-bg/eLibraryWebProject) 
![License](https://img.shields.io/badge/license-MIT-green)

# eLibrary Application
This is my first project and was developed as a final project, part of ASP.NET Advanced course at SoftUni.

## Overview
Welcome to eLibrary, your comprehensive solution for managing books, authors, categories, editions, ratings, and facilitating book borrowing and return processes seamlessly. Whether you're a book enthusiast, a librarian, or a developer looking for a robust library management system, eLibrary has got you covered.

## Features
- Book Management: Effortlessly organize and maintain a database of books with details including title, author, category, edition, ratings and more. (Admin)
- Author Management: Keep track of authors and their works, making it easier to explore their literary contributions. (Admin)
- Category Management: Categorize books efficiently to aid users in discovering content tailored to their interests. (Admin)
- Edition Management: Manage different editions of books, ensuring accurate information and availability. (Admin)
- Rating System: Enable users to rate books and leave comments, fostering community engagement and helping others make informed reading choices. (User)
- Borrowing & Returning Books: Streamline the borrowing and returning processes for library patrons, enhancing user experience. (User)
- Search & Filtering: Empower users to find desired books or authors quickly through robust search and filtering functionalities. (Admin and User)
- User Management: Administer user accounts and permissions, ensuring secure access and data integrity. (Admin)
- Content: Enrich your library experience by uploading pictures for books and authors, bringing the literary world to life with visual storytelling. (Admin)
- Textual Content Support: Expand your literary horizons by uploading text files for books and editions, ensuring that every word is preserved in its original form. (Admin)
- Responsive Design: Enjoy a user-friendly experience across various devices with a responsive design. (Admin and User)
- Admin Superpowers: Admins wield the power to oversee, delete, promote, and demote to admin regular users, ensuring smooth operation and user management. (Admin)
- Insightful System Metrics: Admins can gain valuable insights into system performance and usage through comprehensive metrics, empowering informed decision-making. (Admin)

## Get Started 

Add your connection string in the `appsettings.json` file in the `LibraryManagementSystem.Web` project. Example:

`{
  "ConnectionStrings": {
    "DefaultConnection": "Server=*CHANGE*;Database=eLibrary;User Id=*CHANGE*;Password=*CHANGE*;TrustServerCertificate=True;"
  }
}`

Fire the app from eLibrary_Dev profile in Development mode. This initiates all migrations to the database seamlessly and populates your database with information about books, authors, categories, users and more. Also there is going to be seeded an Administrator (Librarian) and regular User with the provided credentials.

***Administrator***
- Username: Admin-Username
- <details><summary>password: Click to reveal!</summary>pass.123</details>

***User***
- Username: User-Username
- <details><summary>password: Click to reveal!</summary>pass.123</details>

## Users (Roles) ##

The application has three types of users:

- Guests: Explore the home page, search pages, and registration page. Full user functionality is available after registration.
- Users: Gain full access to all application functionalities upon logging in.
- Administrators: Access a special dashboard for creating and managing library information and assets, ensuring smooth operation and user management.

## Software/Technologies Used

- [VS 2022](https://visualstudio.microsoft.com/vs/)
- [MS SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [ASP.NET Core 8.0](https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-8.0)
- [Entity Framework Core 8.0.1](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/8.0.1)
- [HTML](https://developer.mozilla.org/en-US/docs/Web/HTML) / [CSS](https://developer.mozilla.org/en-US/docs/Web/CSS) / [Bootstrap 5.1](https://getbootstrap.com/docs/5.1/getting-started/introduction/)
- [JavaScript](https://www.javascript.com/) / [jQuery](https://jquery.com/)
- [toastr.js](https://github.com/CodeSeven/toastr)
- [HtmlSanitizer 8.0.843](https://github.com/mganss/HtmlSanitizer)
- [Recaptcha v3 2.3.0](https://developers.google.com/recaptcha/docs/v3)
- [Font Awesome](https://fontawesome.com/)
- [NUnit 3.14.0](https://github.com/nunit/nunit)
- [Moq 4.20.70](https://github.com/moq)

## Future Development & Improvements:

As eLibrary continues to evolve, we're committed to enhancing your reading experience and introducing exciting new features. Here's a glimpse into what the future holds:

- Enhanced Book Access Control: Introducing a new module that allows users to read books directly within the browser, without the option to download or copy text. This feature aims to provide a secure and seamless reading experience while safeguarding intellectual property.
- Subscription System for Book Borrowing: Implementing a subscription-based system for book borrowing, where users can access a predetermined number of books concurrently based on their subscription tier. This system will offer flexibility and convenience while ensuring fair usage policies.

## License 

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE)
