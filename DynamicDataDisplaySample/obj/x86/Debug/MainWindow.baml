   M S B A M L   `   `   `  ����  DynamicDataDisplaySample)    #DynamicDataDisplaySample.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Y UDynamicDataDisplay, Version=0.3.0.0, Culture=neutral, PublicKeyToken=5b7d744a7263923f=d34http://research.microsoft.com/DynamicDataDisplay/1.0  5   	     � Title$  
MainWindow��5   	   $	��600��6   $	��800��6)   .��5      �  � RowDefinitions 5   
   �� 5      $��25 �6   �� 5	      $��* �6   �� 5
      $��25 �6   �� 5      $��25 �6   �� 5      $��25 �6   �� 5      $��25 �6   �� 5      $��25 �6   �� 5      $��25 �6   .+�5   
   8  2Microsoft.Research.DynamicDataDisplay.ChartPlotter  -   5          Name$ plotter��5      $��1��6+   $��1��68   3  -Microsoft.Research.DynamicDataDisplay.Plotter   Children. 5      =  7Microsoft.Research.DynamicDataDisplay.MarkerPointsGraph  -   5        $��_MarkerGraph��5   #   2  ,Microsoft.Research.DynamicDataDisplay.Header  5      $��Georgia)�6   $��Voltage chart��6-   =  7Microsoft.Research.DynamicDataDisplay.VerticalAxisTitle  5      $��Georgia)�6#   $��Voltage [V]��68   ?  9Microsoft.Research.DynamicDataDisplay.HorizontalAxisTitle  5      $��Georgia)�6%   $
��Time��6:   5      �� 5   
   -   5        $��
btnGetLine��5      $��2��6#   $��Iscrtaj liniju��60   6I   �� 5   
   -   5        $��btnGetMarkers��5      $��3��6&   $��Iscrtaj markere��63   6M   �� 5   
   -   5        $��btnAnimateLine��5      $��4��6'   $��Animiraj liniju��64   6N   �� 5   
   -   5        $��btnAnimateMarkers��5      $��5��6*   $��Animiraj markere��67   6R   �� 5   
   -   5        $��btnStopTimer��5      $��6��6%   $��Zaustavi tajmer��62   6L   � 5   
   -   5        $
��pbar��5      $��15��5        � Minimum$ 0Q�5!       � Maximum$	 100Q�5"       � Value$  0Q�5#      $��7��5$      5'      5(      