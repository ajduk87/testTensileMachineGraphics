   M S B A M L   `   `   `  ���� Y  UDynamicDataDisplay, Version=0.3.0.0, Culture=neutral, PublicKeyToken=5b7d744a7263923fk:clr-namespace:Microsoft.Research.DynamicDataDisplay.Charts,Microsoft.Research.DynamicDataDisplay.Charts  H    BMicrosoft.Research.DynamicDataDisplay.Charts.Shapes.DraggablePoint   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    Gd3:clr-namespace:Microsoft.Research.DynamicDataDisplay.Charts      � ToolTip  �� *
Position+ �� RelativeSource �� *Self+����  �� 
TargetTypeF   @Microsoft.Research.DynamicDataDisplay.Charts.ViewportUIContainer"   �� 	Resources  	  story �� TargetProperty W� From	 W� To g� Duration g� AutoReverse	 g� RepeatBehavior��%Z   &	        �� .��W� $ Opacity�$ 1X�$	 0.1X�$ 0:0:0.4D� . $	 Forever��.��� 
 �� Property
 �@J�Opacity �� Value$ 1Q��� 
 �@�Cursor$ 	ScrollAll���� 
 � ��$ Center=��� 
 � ��$ Center=� �� Triggers ]�  ]� 
Conditions ��  �� Property �@J�IsMouseOver �� Value . ��  �@J�IsMouseCaptured .  /� N� EnterActions ��  ��Name$ 
storyboard��#��[   N� ExitActions ��  �� BeginStoryboardName$ 
storyboard��.��� $��12��$��12��.+�A� $��3��$��3��
���   �A� $��12��$��12��
������ 
���   �