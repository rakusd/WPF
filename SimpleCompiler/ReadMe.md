SimpleCompilerManual
  - You can open new file by clicking File->New or Ctrl+N.
  - If any changes were made to any open files, closing them will prompt message asking user if he wants to save changes before closing it.
  - You can save file by clicking File->Save as or File->Save (Ctrl+S)
  - You can also open exisiting file by clicking File->Open file (Ctrl+O)
  - You can also open whole C# project by selecting directory with .csproj in it. The project tree will be displayed in the light blue area in the right side of the window.
  - You can open .cs files from the project tree by doubleclicking them.
  - Compiler implements 2 plugins that can be selected in the plugin tab:
    - Syntax Highlight Plugin 1: makes keywords blue and bold, keywords:get, set, public, private
    - Syntax Highlight Plugin 2: makes keywords red and bold, keywords: using, if, else, for
  - User can define his own Plugins implemented as class library that need to be generated into .dll files and placed into same directory as executable file of compiler. They will be loaded into compiler on application launch.
  - User defined Plugins need to implement interface
        public interface IPlugin
        {
            string Name { get; } //name of the plugin
            void Do(RichTextBox richTextBox); //how it should alter the text of file
        }
  - If any project is open. It can be built and run. Compiler will use .sln file to either build the project or to build and run it. User has to choose in the combobox whether he wants to just build project or also run it. Then he can press Execute button or F5.
  - Build output and error list are displayed in the bottom of the window. Output is displayed as the text in the first tab and error list is displayed as a listbox containing error information and error line in code. 
