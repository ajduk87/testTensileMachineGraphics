   M S B A M L   `   `   `  ����   testTensileMachineGraphics1    +testTensileMachineGraphics.GraphicAnimation   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Y UDynamicDataDisplay, Version=0.3.0.0, Culture=neutral, PublicKeyToken=5b7d744a7263923f=d34http://research.microsoft.com/DynamicDataDisplay/1.0  .��� -   $��gridTab2Podtab2��   	Gradient5#��[    � ColumnDefinitions  �� $��10 �$��10���� $
��0.6* ��� $��1 ��� $
��0.4* ��� $��10 �$��10��.+�� -   $��gridonlineGraphic��$��1�� � RowDefinitions �� $��35 ��� $	��750 ��� $��10 ��� $��50 �$��50��$��50���� $��10 ��� $��45 �$��45��$��45��.+�8  2Microsoft.Research.DynamicDataDisplay.ChartPlotter  -     Name$ plotter��$��1��$��1��
�������$	��750��$	��750��$
��1330��$
��1330��$	��Top=�$
��Left=�3  -Microsoft.Research.DynamicDataDisplay.Plotter   Children. =  7Microsoft.Research.DynamicDataDisplay.MarkerPointsGraph  -   $��_MarkerGraph��2  ,Microsoft.Research.DynamicDataDisplay.Header  $��Times New Roman)�$��Grafik��=  7Microsoft.Research.DynamicDataDisplay.VerticalAxisTitle  $��Times New Roman)�$��R [MPa]��?  9Microsoft.Research.DynamicDataDisplay.HorizontalAxisTitle  $��Times New Roman)�$��e [%]��� -   $��gridLabelsParameters��$��3��  �� $��35 �$��35���� $��* �$��85��$��85���� $��30 �$��30���� $��* �$��85��$��85���� $��30 �$��30���� $��* �$��85��$��85���� $��30 �$��30���� $��* �$��95��$��95���� $��30 �$��30���� $��* �$��95��$��95���� $��30 �$��30���� $��* �$	��100��$	��100���� $��30 �$��30���� $��* �$	��100��$	��100���� $��30 �$��30���� $��* �$��85��$��85���� $��30 �$��30���� $��* �$��85��$��85���� $��30 �$��30���� $��* �$��85��$��85���� $��30 �$��30���� $��* �$��25��$��25���� $��35 �$��35��.+��� $��1��$��1,0,1,1q�
����·�$��5q�
�������$
��Auto�� J� SnapsToDevicePixels . $��2���� $��3��$��1,0,1,1q�
����·�$��5q�
�������$
��Auto�� . $��2���� $��5��$��1,0,1,1q�
����·�$��5q�
�������$
��Auto�� . $��2���� $��7��$��1,0,1,1q�
����·�$��5q�
�������$
��Auto�� . $��2���� $��9��$��1,0,1,1q�
����·�$��5q�
�������$
��Auto�� . $��2���� $��11��$��1,0,1,1q�
����·�$��5q�
�������$
��Auto�� . $��2���� $��13��$��1,0,1,1q�
����·�$��5q�
�������$
��Auto�� . $��2���� $��15��$��3��$��1,0,1,1q�
����·�$��5q�
�������$
��Auto�� . $��2���� $��19��$��3��$��1,0,1,1q�
����·�$��5q�
�������$
��Auto�� . $��2���� -   $��lblForceInN��$��Sila        [kN]��$
��Wrap=�$��Center=�$��Center=�$��1��$��55��$��35���� -   $��lblMaxForceInKN��$��Max sila [kN]��$
��Wrap=�$
��Left=�$��Center=�$��3��$��55��$��35��$��10,0,0,0q��� -   $��lblPreassureInMPa��$��60��$
��Wrap=�$��Napon [MPa]��$
��Left=�$��Center=�$��5��$��35��$��10,0,0,0q��� -	   $��lblElongationInMM��$��68��$
��Wrap=�$��Izduženje [mm]��$
��Left=�$��Center=�$��7��$��35��$��8,0,0,0q��� -
   $��lblElongationInProcent��$��68��$
��Wrap=�$��Izduženje [%]��$
��Left=�$��Center=�$��9��$��35��$��8,0,0,0q��� -   $��lblDeltaPreassure��$y�Visible=�$��75��$
��Wrap=�$��Pr napona [MPa/s]��$
��Left=�$��Center=�$��11��$��10,0,0,0q�$��35���� -   $��lblMaxDeltaPreassure��$y�Visible=�$��75��$
��Wrap=�$��Max pr nap [MPa/s]��$
��Left=�$��Center=�$��13��$��35��$��10,0,0,0q��� -   $��lblDeltaElongation��$y�Visible=�$��10,0,0,0q�$	��166��$
��Wrap=�$*��$Brzina relativne deformacije   [1/s]��$
��Left=�$��Center=�$��15��$��4��$��35���� -   $��lblMaxDeltaElongation��$y�Visible=�$��10,0,0,0q�$	��166��$
��Wrap=�$$��Max Promena izduženja   [1/s]��$
��Left=�$��Center=�$��19��$��2��$��33��� -   $��gridTextboxParameters��$��5��  �� $��35 �$��35���� $��* �$��85��$��85���� $��30 �$��30���� $��* �$��85��$��85���� $��30 �$��30���� $��* �$��85��$��85���� $��30 �$��30���� $��* �$��95��$��95���� $��30 �$��30���� $��* �$��95��$��95���� $��30 �$��30���� $��* �$	��100��$	��100���� $��30 �$��30���� $��* �$	��100��$	��100���� $��30 �$��30���� $��* �$��85��$��85���� $��30 �$��30���� $��* �$��85��$��85���� $��30 �$��30���� $��* �$��85��$��85���� $��30 �$��30���� $��* �$��85��$��85���� $��35 �$��35��.+��� -   $��tfForceInkN��$	��0.0��$��20(�$��1��
�������$��Stretch=�$��Center=� �� 
IsReadOnly . �� -   $��tfMaxForceInKN��$	��0.0��$��20(�$��3��
�������$��Stretch=�$��Center=� . �� -   $��tfPreassureInMPa��$	��0.0��$��20(�$��5��
�������
���   �$��Stretch=�$��Center=� . �� -   $��tfElongationInMM��$	��0.0��$��20(�$��7��
�������
���   �$��Stretch=�$��Center=� . �� -   $��tfElongationInProcent��$	��0.0��$��20(�$��9��$��Stretch=�$��Center=�
�������
���   � . �� -   $��tfDeltaPreassure��$y�Visible=�$	��0.0��$��20(�$��11��$��Stretch=�$��Center=�
�������
����  � . �� -   $��tfMaxDeltaPreassure��$y�Visible=�$	��0.0��$��20(�$��13��$��Stretch=�$��Center=�
�������
����  � . �� -   $��tfDeltaElongation��$y�Visible=�$��15��$��3��$	��0.0��$��20(�$��Stretch=�$��Center=�
���   �
���   �
����� � . �� -   $��tfMaxDeltaElongation��$y�Visible=�$��19��$��3��$	��0.0��$��20(�$��Stretch=�$��Center=�
���   �
���   �
����� � . �� -   $��lblAnimationStatus��$��	Animacija��$��Center=�$��Center=�$��0��$��1��$��20(�� -   $��gridanimationRightPart��$��3�� �� $��35 ��� $��35 ��� $��35 ��� $��35 ��� $��35 ��� $��35 �.+�� -   $��gridAnimationFilePath��$��0��$��1��  �� $��10 �$��10���� $
��Auto ��� $��10 �$��10���� $��30 �$��30���� $��10 �$��10��.+��� -   $��lblFilepath��$��1��$��0��$��Center=�$E��?Putanja fajla koju želite da prikažete u animacionom modu :  ���� $��3��$��Center=�$��Center=�  Flat3DBorderBrush#��[ $��5q����#BBB._��� -   $��btnChooseDatabasePath��$	��...��$��20��$��20��$��Center=�$��Center=�$��1��$��0��� -   $#��gridAnimationFilePathTextBox2��$��0��$��2��  �� $��10 �$��10���� $
��Auto �.+��� -   $��tfFilepathAnimation��$��1��$��0��  textboxStyle1#��[ $
��Left=� . $��25��$	��510��� -    $��gridAnimationButton2��$��0��$��3��  �� $��10 �$��10���� $
��Auto ��� $��10 �$��10���� $��30 �$��30���� $��10 �$��10��.+��� $��1��$��0��$��Center=�$��Center=�#��[ $��5q����#BBB._��� -!   $��btnAnimationMode��$��0��$��1��$	��250��$��30��$��20(�$
��Bold"�$
��Left=�$��Center=�$��POKRENI ANIMACIJU���� -"   $+��%chbStartSampleShowChangedParAnimation��$��15��$��3�� � ToolTip$2 ,Prikaži grafike promene napona i izduženja��$��Center=�$
��Left=�