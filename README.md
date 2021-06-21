[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]

<!-- PROJECT LOGO -->
<br />
<p align="center">
  
  <a href="https://github.com/entvex/NgrokGUI">
    <img src="icons8-tunnel-256.png" alt="Logo" width="80" height="80">
  </a>

  <h3 align="center">NgrokGUI</h3>

  <p align="center">
    NgrokGUI is a windows GUI for ngrok
    <br />
    <br />
    <a href="https://github.com/entvex/NgrokGUI/issues">Report Bug</a>
    ·
    <a href="https://github.com/entvex/NgrokGUI/issues">Request Feature</a>
  </p>
</p>

<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary><h2 style="display: inline-block">Table of Contents</h2></summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Download and use</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgements">Acknowledgements</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->
## About The Project

NgrokGUI is a windows GUI for ngrok. It helps download ngrok, getting it ready for first time use and managing tunnels. The idea for the project arose due to friends wanting a gui to ngrok that was more user friendly.
I later found a Dungeons & Dragons community that uses [Foundryvtt](https://foundryvtt.com/) to run their games. NgrokGUI allows an easy way for the dungeon master to host the game without spending additional money on hosting or messing with port forwarding.

#### 🔨 New version underway! 🔨
I have gotten requests for this application to be ported to Linux and Mac as well. Therefore, I am in the process of rewriting the application in avaloniaui.
Look at the v2 branch if you wanna follow the progress

### Built With

* [NgrokSharp](https://github.com/entvex/NgrokSharp)

<!-- GETTING STARTED -->
## Getting Started

To get a local copy up and running follow these simple steps.

### Prerequisites

* Windows 7 or Later
* [.NET 5.0 x64](https://dotnet.microsoft.com/download/dotnet/5.0/runtime/) or later
* [Ngrok authtoken](https://dashboard.ngrok.com/get-started/setup)
### Installation

1. Go to the [releases](https://github.com/entvex/NgrokGUI/releases) section.
2. Download the latest NgrokGUI.zip and unzip it a place were you would like to keep it.
3. Run the NgrokGUI.exe and follow the First Time Wizard on screen.

<!-- USAGE EXAMPLES -->
## Usage

This example will show how to use NgrokGUI to share a [Foundryvtt](https://foundryvtt.com/) session.

1. Make sure NgrokGUI is started and the First Time Wizard is complete.
2. Click the File button in the upper left and click New to open the Add New Tunnel window.
3. Enter a name such as Foundryvtt and make sure the Protocol https is selected. Foundryvtts default port is 30000 add that to the Local port field.
4. Click the Add new tunnel button.
5. In the main window right click the tunnel with your chosen name and click copy link to share it with the people who need access.

![ShowCaseOfStep1to4](https://i.imgur.com/mjJ2WN5.gif)

<!-- ROADMAP -->
## Roadmap

See the [open issues](https://github.com/entvex/NgrokGUI/issues) for a list of proposed features (and known issues).

<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.

<!-- CONTACT -->
## Contact

Project Link: [https://github.com/entvex/NgrokGUI](https://github.com/entvex/NgrokGUI)

<!-- ACKNOWLEDGEMENTS -->
## Acknowledgements
Thanks to these
* [Icons8 for icons](https://icons8.com/)
* [Ngrok for testing account](https://ngrok.com/)

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/entvex/NgrokGUI.svg?style=for-the-badge
[contributors-url]: https://github.com/entvex/NgrokGUI/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/entvex/NgrokGUI.svg?style=for-the-badge
[forks-url]: https://github.com/entvex/NgrokGUI/network/members
[stars-shield]: https://img.shields.io/github/stars/entvex/NgrokGUI.svg?style=for-the-badge
[stars-url]: https://github.com/entvex/NgrokGUI/stargazers
[issues-shield]: https://img.shields.io/github/issues/entvex/NgrokGUI.svg?style=for-the-badge
[issues-url]: https://github.com/entvex/NgrokGUI/issues
[license-shield]: https://img.shields.io/github/license/entvex/NgrokGUI.svg?style=for-the-badge
[license-url]: https://github.com/entvex/NgrokGUI/blob/master/LICENSE.txt
