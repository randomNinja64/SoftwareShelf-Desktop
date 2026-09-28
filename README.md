<p align="center">
	<img src="softwareshelf.ico" alt="SoftwareShelf Desktop Icon" width="120" />
</p>
<h3 align="center">SoftwareShelf Desktop</h3>
<p align="center"><em>Internet Archive Download Manager</em></p>

## About

SoftwareShelf Desktop is a desktop application designed to search and download files from the [Internet Archive](https://archive.org). The application targets Windows XP, but supports Windows 98.

## Requirements

- Windows 98 or later (Windows XP+ for multithreaded Aria2-based downloads)
- .NET Framework 2.0 (on Windows 7 and later, the TLS 1.2 update to .NET 3.5 lets single-threaded downloads use TLS 1.2)
- aria2c executable (included with installer and release builds but not with source code)

## Building

The software can be built by building the solution in Visual Studio or by running `build.bat`. Builds packaged via build.bat will automatically package Aria2 if `aria2c.exe` is present in the `deps` folder). Additionally, if NSIS is available, an installer will be built.

## Usage

- **Search Tab**
  - **Search Box**
    - **Keyword**, **Creator**, **Topic**, **Publication Year** - fields to search the Internet Archive by
	- **Type** - Internet Archive item type (`All`, `Audio`, `Books`, `Images`, `Movies`, or `Software`; default `Software`)
	- **Latest Items for Type** - loads the newest items for the selected type
	- **Search** - performs a search
  - **Results Grid** - Results are shown in a grid containing their name, rating, size (in KB), and number of downloads.
  - **Result Preview** - Result previews are shown on the right side and contain a thumbnail, creator, published year, topic, and description.
    - **Reviews** - opens a form showing user reviews
    - **Download** - opens a form allowing for files to be selected/downloaded
    - **ZIP** - downloads full archive item as a single ZIP file (only supported for items under 40 GB)
- **Downloads Tab**
  - **Downloads Grid** - Downloads are shown in a grid containing their file name, speed, and progress.
  - **Downloads Directory** - shows the current directory for downloaded files
  - **Aria2** (Windows XP+) - enables/disables Aria2 multithreaded downloads
    - **Process Torrents**  - enables/disables Aria2 torrent processing
    - **Threads** -  number of threads for multithreaded downloads
  - **Open Downloads** - Opens the downloads folder in Windows Explorer
  - **Browse** - Sets the downloads folder (also set on first launch)
  - **Cancel** - Cancels the selected download

## Credits

- Aria2 builds are provided by [dmesg00](https://github.com/dmesg00/aria2-static-builds)

## License

SoftwareShelf Desktop is licensed under the MIT license. For third party executables, see licenses under THIRD_PARTY_LICENSES.
