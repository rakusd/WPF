   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	       WpfApp1.MailWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp1     5   	   -   5          � Title$  
MailWindow��5   	   $	��300��6   $	��500��6)    � WindowStartupLocation$ CenterOwner=�5	   	    � WindowStyle$
 None=�6-    � ShowInTaskbar .   � 
ResizeMode$ NoResize=�5
   	    � 	Resources 5       �� MergedDictionaries �� Source �� 
TargetType	 �� Triggers
 P� Property P� Value �� Property �� Value �� CornerRadius � ToolTip �� ElementName �� Path�� 5   
   -   5        LanguageDictionary�� 5      �� 5      $ English.xaml?�6%   %�  '��       �� 5      " ��6   	 5      P� 5      
 �@=�HasError . . �5      ��  � ��
 �  ��68   �� 5       � ��$ 3q�6<   5      .�5      ��  �@=�ErrorTemplate 5      �� 5      .R�5      ]� ��. .+�5   "   �� 
���  ��$��20��6�   $��20��6�   $ 5��6�   $��Right=�6�    6)   �� $ AdornedElement��$8 2AdornedElement.(Validation.Errors)[0].ErrorContent�._�5   &   �� $��Center=�60   $��Center=�6K   
�������$��12(�5   3   $
��Bold"�5        X5"   '   5#   #   �� 5$   "   ._�5%   &   �� $��AdornedElement��6@   5&   #   5'      5(      5*      5+      5,      .��5/      ]� .+�50   
   �� $	��100��6   $	��Top=�6   ._�51      ��    
StrNewMail#���   6   $��25(�6<   
��� d �$��Center=�52   /   $��30,0,0,0q�53      54      � 55   
   $��30��6   $	��Top=�6    � ColumnDefinitions 56      �� 57      $	��100 �6#   �� 58      .+�5:      �� $��0,0,10,0q�6    	 StrTo#���  6*   $��Right=�6I   �� 5;      -   5        $��MailTo��5;      $��1��6%   $��	0,0,30,10q�65   .��6H   ��5<      �� 5=      $ To�6    �� UpdateSourceTrigger$ PropertyChanged=�6(    �� ValidationRules 5>         WpfApp1.ToValidator  5?         
EmptyError$ Empty��5@   !      IncorrectError$ 	Incorrect��5A   !   5C      .��5D       5E      5F      � 5H   
   $��30��6   $	��Top=�6    5I      �� 5J      $	��100 �6#   �� 5K      .+�5N      �� $��0,0,10,0q�6     StrTitle#���  6*   $��Right=�6L   �� 5O      -   5        $��	MailTitle��5O      $��1��6'   $��	0,0,30,10q�67   .��6J   ��5P      �� 5Q      $ Titled�6   $ PropertyChanged=�6,    5R         WpfApp1.TitleValidator  5S         
EmptyError$ Empty��5T   !   5V      .��5W       5X      5Y      � 5[   
   $��Bottom=�6   $��30��6'   .+�5\      �� -   5        $��60��5\      $��20��6    $��Right=�6,   $��0,0,20,0q�6H   ��5]      �� 5^      " ��6   .�5_      ��  � {� . 66   	 5`      �� 5a       �� Binding 6*   �� $ 	MailTitle��$ (Validation.HasError)� �� Value True6o   .a�5b   "   ��  � {� .  6>   5c      �� 5d       6*   �� $ MailTo��$ (Validation.HasError)� True6l   .a�5e   "   ��  � {� .  6>   5f      �� 5g       6*   �� $ MailBody��$ (Validation.HasError)� True6n   .a�5h   "   ��  � {� .  6>   5i      �� 5j       6*   �� $ 	MailTitle��$
 Text�$ PropertyChanged=�  6�   .a�5k   "   ��  � {� .  6>   5l      �� 5m       6*   �� $ MailTo��$
 Text�$ PropertyChanged=�  6   .a�5n   "   ��  � {� .  6>   5o      �� 5p       6*   �� $ MailBody��$
 Text�$ PropertyChanged=�  6�   .a�5q   "   ��  � {� .  6>   5r      5t      .��5v      ��   StrSend#���  6   5w      �� 5x      -   5        $��60��5x      $��20��6    $��Right=�6,   $��	0,0,100,0q�6H   .��5y      ��   	StrCancel#���  6   5z      5{      � 5}   
   $��Stretch=�6   $	��Top=�6,    5~      �� 5      $	��100 �6#   �� 5�      .+�5�      �� $��0,0,10,0q�6     
StrMessage#���  6*   $��Right=�6N   �� 5�      -   5        $��MailBody��5�       �� TextWrapping$
 Wrap=�6&   $��1��6:   $��	0,0,30,10q�6J   .��6]   ��5�      �� 5�      $
 Body�6   $ PropertyChanged=�6*    5�         WpfApp1.BodyValidator  5�         
EmptyError$ Empty��5�   !     StrTooShort   LengthError# [ 5�   !   5�      .��5�       5�      5�      5�      5�      