   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	       WpfApp1.LoginWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp1     5   	     � Title$  LoginWindow��5   	   $	��200��6   $	��400��6*    � WindowStartupLocation$ CenterOwner=�5	   	    � WindowStyle$
 None=�6-    � ShowInTaskbar .   � 
ResizeMode$ NoResize=�5
   	    � 	Resources 5       �� MergedDictionaries �� Source   ButtonEnabler �� 
TargetType�� 5   
   -   5        LanguageDictionary�� 5      �� 5      $ English.xaml?�6%   %+   &	        �� 5      " ��6*   5      5      .��5      � 	 � RowDefinitions	 5   
   �� 5      $��50 �6   �� 5      $	��100 �6   �� 5       $��50 �6   .+�5"   
   �� $��25(�6   
��� d �  StrLogin#���  65   6\   �� 5#   
   -   5        $��Username��5#      $��1��6"   $��25��6/   $	��240��6;   $	��Top=�6G   $
��Left=�6_   $��
100,20,0,0q�6z   5$      �� 5%   
   $��1��6   $��30��6   $��85��6)   $	��Top=�64   $
��Left=�6L   $��	10,15,0,0q�6g     StrUsername#���  6z   6�   G� 5'   
   -   5        $��Password��5'      $��1��6&   $��25��63   $	��240��6?   $	��Top=�6K   $
��Left=�5(      $��
100,60,0,0q�61   5)      �� 5*   
   $��1��6   $��30��6   $��85��6)   $	��Top=�64   $
��Left=�6L   $��	10,55,0,0q�6g     StrPassword#���  6z   �� 5,   
   -   5        $��2��5,      $��20��6   $��80��6*   $��
-100,0,0,0q�65   
��� d �  	StrCancel#���  6u   �� 5-   
   -   5        $��OKButton��5-      $��2��6!   $��20��6.   $��80��6:   $��	100,0,0,0q�6E   
��� d � 	 StrOK#���  5.   (   #��[  5/      {�.  68   50      51      