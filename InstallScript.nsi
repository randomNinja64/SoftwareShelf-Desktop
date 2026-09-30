; SoftwareShelf Desktop NSIS installer
; Built by build.bat when makensis is available.
; Packages the staged release: the app, Aria2 when present, and THIRD_PARTY_LICENSES.

!define PRODUCT_NAME "SoftwareShelf Desktop"
!define PRODUCT_VERSION "1.7.0"
!define PRODUCT_PUBLISHER "SoftwareShelf"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\SoftwareShelf Desktop.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

!ifndef DIST_DIR
  !define DIST_DIR "SoftwareShelf Desktop"
!endif
!ifndef SETUP_OUT
  !define SETUP_OUT "SoftwareShelf Desktop Setup.exe"
!endif

SetCompressor lzma

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "${SETUP_OUT}"
LoadLanguageFile "${NSISDIR}\Contrib\Language files\English.nlf"
InstallDir "$PROGRAMFILES\SoftwareShelf Desktop"
Icon "softwareshelf.ico"
UninstallIcon "softwareshelf.ico"
InstallDirRegKey HKLM "${PRODUCT_DIR_REGKEY}" ""
DirText "Setup will install $(^Name) in the following folder.$\r$\n$\r$\nTo install in a different folder, click Browse and select another folder."
LicenseText "If you accept all the terms of the agreement, choose I Agree to continue. You must accept the agreement to install $(^Name)."
LicenseData "LICENSE"
ShowInstDetails show
ShowUnInstDetails show

Section "MainSection" SEC01
  SetOutPath "$INSTDIR"
  SetOverwrite ifnewer
  File "${DIST_DIR}\SoftwareShelf Desktop.exe"
  File "${DIST_DIR}\SoftwareShelf Desktop.exe.config"
  File "${DIST_DIR}\Newtonsoft.Json.dll"
  !ifdef HAVE_ARIA2
    File "${DIST_DIR}\aria2c.exe"
  !endif
  !ifdef HAVE_LICENSES
    File /r "${DIST_DIR}\THIRD_PARTY_LICENSES"
  !endif
  CreateDirectory "$SMPROGRAMS\SoftwareShelf Desktop"
  CreateShortCut "$SMPROGRAMS\SoftwareShelf Desktop\SoftwareShelf Desktop.lnk" "$INSTDIR\SoftwareShelf Desktop.exe"
  CreateShortCut "$DESKTOP\SoftwareShelf Desktop.lnk" "$INSTDIR\SoftwareShelf Desktop.exe"
SectionEnd

Section -AdditionalIcons
  CreateShortCut "$SMPROGRAMS\SoftwareShelf Desktop\Uninstall.lnk" "$INSTDIR\uninst.exe"
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\SoftwareShelf Desktop.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\SoftwareShelf Desktop.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd


Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) was successfully removed from your computer."
FunctionEnd

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "Are you sure you want to completely remove $(^Name) and all of its components?" IDYES +2
  Abort
FunctionEnd

Section Uninstall
  Delete "$INSTDIR\uninst.exe"
  Delete "$INSTDIR\Newtonsoft.Json.dll"
  Delete "$INSTDIR\aria2c.exe"
  RMDir /r "$INSTDIR\THIRD_PARTY_LICENSES"
  Delete "$INSTDIR\SoftwareShelf Desktop.exe"
  Delete "$INSTDIR\SoftwareShelf Desktop.exe.config"

  Delete "$SMPROGRAMS\SoftwareShelf Desktop\Uninstall.lnk"
  Delete "$DESKTOP\SoftwareShelf Desktop.lnk"
  Delete "$SMPROGRAMS\SoftwareShelf Desktop\SoftwareShelf Desktop.lnk"

  RMDir "$SMPROGRAMS\SoftwareShelf Desktop"
  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd
