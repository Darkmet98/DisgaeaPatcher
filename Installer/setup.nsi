; Full script for making an NSIS installation package for .NET programs,
; Allows installing and uninstalling programs on Windows environment, and unlike the package system 
; integrated with Visual Studio, this one does not suck.

;To use this script:
;  1. Download NSIS (http://nsis.sourceforge.net/Download) and install
;  2. Save this script to your project and edit it to include files you want - and display text you want
;  3. Add something like the following into your post-build script (maybe only for Release configuration)
;        "$(DevEnvDir)..\..\..\NSIS\makensis.exe" "$(ProjectDir)Setup\setup.nsi"
;  4. Build your project. 
;
;  This package has been tested latest on Windows 7, Visual Studio 2010 or Visual C# Express 2010, should work on all older version too.

; Textos
!define MUI_PAGE_HEADER_TEXT "Instalador del parcheador de Disgaea PC por HyperTraducciones."
!define MUI_PAGE_HEADER_SUBTEXT "Parcheador de Disgaea PC que lo traduce al castellano."
!define MUI_WELCOMEPAGE_TITLE "Bienvenido al instalador del parcheador de Disgaea PC"
!define MUI_WELCOMEPAGE_TITLE_3LINES
!define MUI_WELCOMEPAGE_TEXT "Este instalador contiene el parcheador para aplicar la traducción al castellano de Disgaea PC realizada por Hypertraducciones."

!define MUI_DIRECTORYPAGE_TEXT_TOP "El instalador necesita que especifiques la carpeta donde tengas instalado Disgaea PC para instalar el parcheador."

!define MUI_DIRECTORYPAGE_TEXT_DESTINATION "Selecciona la carpeta donde tengas instalado Disgaea PC"

!define MUI_INSTFILESPAGE_FINISHHEADER_TEXT "Instalando el parcheador de Disgaea PC..."
!define MUI_INSTFILESPAGE_FINISHHEADER_SUBTEXT "Se está instalado el parcheador de Disgaea PC, no cierres el proceso..."

!define MUI_FINISHPAGE_TITLE "Se ha instalado el parcheador."
!define MUI_FINISHPAGE_TEXT "Se ha instalado el parcheador ccorrectamente. Ahora inicie el juego para aplicar el parche.\n\n\nPresione «Terminar» para cerrar este asistente."

; Imágenes
!define MUI_WELCOMEFINISHPAGE_BITMAP "welcome.bmp"

; Main constants - define following constants as you want them displayed in your installation wizard
!define PRODUCT_NAME "Parcheador Disgaea PC"
!define PRODUCT_VERSION "castellano 1.0"
!define PRODUCT_WEB_SITE "https://hypertraducciones.blogspot.com/"
!define PRODUCT_PUBLISHER "HyperTraducciones"
!include 'LogicLib.nsh'

; Following constants you should never change
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKCU"

!include "MUI.nsh"
!define MUI_ABORTWARNING
!define MUI_ICON "icon.ico"
!define MUI_UNICON "uninstaller.ico"

RequestExecutionLevel admin #NOTE: You still need to check user rights with UserInfo!

; Wizard pages
!insertmacro MUI_PAGE_WELCOME
; Note: you should create License.txt in the same folder as this file, or remove following line.
!insertmacro MUI_PAGE_LICENSE "License.txt"
!define MUI_PAGE_CUSTOMFUNCTION_LEAVE CheckGame
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_LANGUAGE "Spanish"

; Replace the constants bellow to hit suite your project
Caption "Instalador del parcheador de Disgaea PC al castellano por HyperTraducciones."
Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "Instalador parcheador Disgaea PC.exe"
InstallDir "$PROGRAMFILES32\Steam\steamapps\common\Disgaea PC\"
ShowInstDetails show
ShowUnInstDetails show

; ValidateDir
Function CheckGame
${IfNot} ${FileExists} "$InstDir\*"
    MessageBox MB_OK `¡La carpeta seleccionada no existe!`
    Abort
${EndIf}

${IfNot} ${FileExists} "$InstDir\dis1_st.exe"
    MessageBox MB_OK `¡No se ha detectado una instalación válida de Disgaea PC!`
    Abort
${EndIf}

FunctionEnd


; Following lists the files you want to include, go through this list carefully!
Section "MainSection" SEC01

	Rename $INSTDIR\dis1_st.exe $INSTDIR\dis1_st_en.exe
	
	SetOutPath "$INSTDIR"
	SetOverwrite ifnewer
	File "..\DisgaeaPatcher\bin\Desktop\win-ia32-unpacked\*"

	CreateDirectory $INSTDIR\locales
	SetOutPath $INSTDIR\locales
	SetOverwrite ifnewer

	File "..\DisgaeaPatcher\bin\Desktop\win-ia32-unpacked\locales\*"

	CreateDirectory $INSTDIR\resources
	SetOutPath $INSTDIR\resources
	SetOverwrite ifnewer

	File "..\DisgaeaPatcher\bin\Desktop\win-ia32-unpacked\resources\*"

	CreateDirectory $INSTDIR\resources\bin
	SetOutPath $INSTDIR\resources\bin
	SetOverwrite ifnewer

	File "..\DisgaeaPatcher\bin\Desktop\win-ia32-unpacked\resources\bin\*"

	CreateDirectory $INSTDIR\resources\bin\wwwroot\audio
	SetOutPath $INSTDIR\resources\bin\wwwroot\audio
	SetOverwrite ifnewer

	File "..\DisgaeaPatcher\bin\Desktop\win-ia32-unpacked\resources\bin\wwwroot\audio\*"
	
	CreateDirectory $INSTDIR\resources\bin\wwwroot\css
	SetOutPath $INSTDIR\resources\bin\wwwroot\css
	SetOverwrite ifnewer

	File "..\DisgaeaPatcher\bin\Desktop\win-ia32-unpacked\resources\bin\wwwroot\css\*"
	
	CreateDirectory $INSTDIR\resources\bin\wwwroot\css\bootstrap
	SetOutPath $INSTDIR\resources\bin\wwwroot\css\bootstrap
	SetOverwrite ifnewer

	File "..\DisgaeaPatcher\bin\Desktop\win-ia32-unpacked\resources\bin\wwwroot\css\bootstrap\*"
	
	CreateDirectory $INSTDIR\resources\bin\wwwroot\img
	SetOutPath $INSTDIR\resources\bin\wwwroot\img
	SetOverwrite ifnewer

	File "..\DisgaeaPatcher\bin\Desktop\win-ia32-unpacked\resources\bin\wwwroot\img\*"
	
	CreateDirectory $INSTDIR\resources\bin\wwwroot\img\Avatares
	SetOutPath $INSTDIR\resources\bin\wwwroot\img\Avatares
	SetOverwrite ifnewer

	File "..\DisgaeaPatcher\bin\Desktop\win-ia32-unpacked\resources\bin\wwwroot\img\Avatares\*"

	CreateDirectory $INSTDIR\swiftshader
	SetOutPath $INSTDIR\swiftshader
	SetOverwrite ifnewer

	File "..\DisgaeaPatcher\bin\Desktop\win-ia32-unpacked\swiftshader\*"
	
	Rename $INSTDIR\DisgaeaPatcher.exe $INSTDIR\dis1_st.exe
	
	; Start Menu
	;createDirectory "$SMPROGRAMS\${PRODUCT_PUBLISHER}"
	;createShortCut "$SMPROGRAMS\${PRODUCT_PUBLISHER}\Disgaea PC castellano.lnk" "$INSTDIR\dis1_st.exe" "" "$INSTDIR\dis1_st.exe"
	;createShortCut "$SMPROGRAMS\${PRODUCT_PUBLISHER}\Desinstalar parche.lnk" "$INSTDIR\uninst.exe" "" "$INSTDIR\uninst.exe"

SectionEnd

Section -Post
  ;Following lines will make uninstaller work - do not change anything, unless you really want to.
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\uninst.exe"
  
SectionEnd

; Replace the following strings to suite your needs
Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "Se ha desinstalado el parcheador y parche de Disgaea PC castellano."
FunctionEnd

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "¿Quieres desinstalar el parcheador y parche de Disgaea PC castellano?" IDYES +2
  Abort
FunctionEnd

; Remove any file that you have added above - removing uninstallation and folders last.
; Note: if there is any file changed or added to these folders, they will not be removed. Also, parent folder (which in my example 
; is company name ZWare) will not be removed if there is any other application installed in it.
Section Uninstall
	SetShellVarContext all
	RMDir /r "$INSTDIR\locales"
	RMDir /r "$INSTDIR\resources"
	RMDir /r "$INSTDIR\swiftshader"
	Delete "$INSTDIR\chrome_100_percent.pak"
	Delete "$INSTDIR\chrome_200_percent.pak"
	Delete "$INSTDIR\d3dcompiler_47.dll"
	Delete "$INSTDIR\DATA.esp"
	Delete "$INSTDIR\dis1_st.exe"
	Delete "$INSTDIR\dis1_st_es.exe"
	Delete "$INSTDIR\ffmpeg.dll"
	Delete "$INSTDIR\icudtl.dat"
	Delete "$INSTDIR\libEGL.dll"
	Delete "$INSTDIR\libGLESv2.dll"
	Delete "$INSTDIR\LICENSE.electron.txt"
	Delete "$INSTDIR\LICENSES.chromium.html"
	Delete "$INSTDIR\resources.pak"
	Delete "$INSTDIR\snapshot_blob.bin"
	Delete "$INSTDIR\SUBDATA.esp"
	Delete "$INSTDIR\v8_context_snapshot.bin"
	Delete "$INSTDIR\version.json"
	Delete "$INSTDIR\vk_swiftshader.dll"
	Delete "$INSTDIR\vk_swiftshader_icd.json"
	Delete "$INSTDIR\vulkan-1.dll"
	Delete "$INSTDIR\uninst.exe"
	Rename "$INSTDIR\dis1_st_en.exe" "$INSTDIR\dis1_st.exe"
	;RMDir /r "$SMPROGRAMS\${PRODUCT_PUBLISHER}"


  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"

  SetAutoClose true
SectionEnd