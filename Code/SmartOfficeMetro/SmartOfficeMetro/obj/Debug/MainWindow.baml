   M S B A M L   `   `   `  ����K  GSmartOfficeMetro, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:SmartOfficeMetroSmartOfficeMetro  5      E'clr-namespace:SmartOfficeMetro.CommandsSmartOfficeMetro.Commands  5      !    SmartOfficeMetro.MainWindow   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      T PMahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f;Controls,http://metro.mahapps.com/winfx/xaml/controls  5      0SmartOfficeclr-namespace:SmartOfficeMetro  5      0fsxaml#http://github.com/fsprojects/FsXaml  5      \ XHardcodet.Wpf.TaskbarNotification, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null)tb http://www.hardcodet.net/taskbar  5      5command'clr-namespace:SmartOfficeMetro.Commands  5      -   5          � Title$  
MainWindow��5	      $	��458��5
      $��784.5��5       � WindowState$ 	Maximized=�6%   (  "MahApps.Metro.Controls.MetroWindow   RightWindowCommands 5      +  %MahApps.Metro.Controls.WindowCommands  5   
   .@�5      �� $5��/pack://siteoforigin:,,,/Resources/icon-gear.png��6   6O   �� 5      -   5        $��Settings��5      6(   5       � 	Resources 5       �� MergedDictionaries�� 5   
    5      %    5      .��5      �  � ColumnDefinitions 5   
   �� 5      �� 5       $��4* �6    � RowDefinitions 5#   
   �� 5$      $
��Auto �6   �� 5%      �� 5&      $
��2.5* �6   �� 5'      $
��Auto �6   .+�5+   
   o� -   5         o�Name$
 menu��5+      $��
0,10,10,10q�6   $��2��61   5.      �� 5/   
   -   5         ��Name$ labelWelcome��5/      $��	Welcome, ��6&   $��1��6:   $��33��6J   $��	10,10,0,0q�6V   $��1��6i   $��Center=�6v   $	��375��6�   $��20(�6�   $
��Left=�6�   �� 50   
   -   5        	 ��Name$	 imageUserIcon��50      $��1��6'   $
��Fill=�64   $��	20,5,21,4q�6]   ��6C   �� *imgSrc+6]   3  -Hardcodet.Wpf.TaskbarNotification.TaskbarIcon  53   
   -   5        
  Name$
 popUp��53      �� 56   
   $��2��6   .+�57   
   �� -   5         ��Name$ buttonDelivery��57      $��Delivery��6)   $��2��6U   $	��Top=�6b   $��2q�6z   �� 58      -   5        $ buttonOrderCoffee��58      $��Order Coffee��60   $��2��6[   $	��Top=�6h   $��2q�6�   �� 59      -	   5        $ buttonNotifications��59      $��Notifications��62   $��2��6d   $	��Top=�6q   $��2q�6�   �� 5:      -
   5        $ buttonAdmin��5:      $��Admin Functions��6*   $��2��6^   $	��Top=�6k   $��2q�6�   o� 5;      -   5        $ 
notifyMenu��5;      $	��100��6'   5<      ]� 5=   
   -   5         ]�Name$ 	dockPanel��5=      $��1��6'   $
��Auto��67   ��. $��2��6Z   $
��Auto��6g   $��Stretch=�6t   $��Stretch=�6�   5?         Flyouts 5A      +  %MahApps.Metro.Controls.FlyoutsControl  5B   
   -   5          Name$ flyoutsControl��5B   "   .@�5D      #  MahApps.Metro.Controls.Flyout     Header$ Settings��6      Position$ Right=�5E         Theme$ Accent=�5F      .��5G      � $��10q�6   $	��400��5H       5I      �� 5J      $
��30px �6(   �� 5K      $
��30px �6(   �� 5L      $
��30px �6(   �� 5M      $
��30px �6(   .+�5O      �� $C��=This flyout has the inverse theme of the current window theme��6    $��0��6e   	� 5P      -   5         	�Name$ radioButtonBlue��5P   "   $��Blue Accent��6;   $
��Left=�6Q   $��1��6�   $��21��6�   $	��109��6�   $��1��6�   	� 5Q      -   5        $ radioButtonRed��5Q   "   $��
Red Accent��6:   $
��Left=�6O   $��1��6�   $��21��6�   $	��104��6�   $��2��6�   	� 5R      -   5        $ radioButtonGreen��5R   "   $��Green Accent��6<   $
��Left=�6S   $��1��6�   $��20��6�   $	��121��6�   $��3��6�   5T      5U      5W      5_      