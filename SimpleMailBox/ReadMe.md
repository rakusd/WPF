SimpleMailBoxManual
Note: This is not a mail management app. It is just a project with 2 predefined users. The aim of this app is to learn aspects of WPF styles.
Predefined users: Username1:wpf Password1:iseasy, Username2:mini Password2:pw
   
   - 2 languages English and Polish are supported
   - After logging in as one of two predefined users you have 2 tabs:one containing received mails and one containing sent mails
   - After selecting one of the emails its content(title, date, sender, message) is displayed in the right window
   - After pressing New Mail button a new form is opened.To send email you need to pass 3 validation rules:
    - To: field cannot be empty and needs to contain at least 1 character then "@" character, then at least 1 character, then "." character and at least 2 characters.
    - Title: field cannot be empty
    - Message: Needs to be at least 10 characters long
    -After passing those rules send button becomes enabled and you can view this message in Sent tab.
    -In the top there is textbox, in which you can type strings separated by spaces. After something is typed in there only emails containing at least one of the words in either title, sender or date are displayed in the tab.
