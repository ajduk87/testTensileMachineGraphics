   M S B A M L   `   `   `  ����   testTensileMachineGraphics0    *testTensileMachineGraphics.GraphicPlotting   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Y UDynamicDataDisplay, Version=0.3.0.0, Culture=neutral, PublicKeyToken=5b7d744a7263923f=d34http://research.microsoft.com/DynamicDataDisplay/1.0  .��� -   $��gridTab2PodTab1��   	Gradient5#��[    � 	Resources    buttonTemplate A�Name �� 
StartPoint �� EndPoint � Offset � Color �� RelativeSource �� Path  buttonTemplateTriangle � Points	 �� 
TargetType
 �� Property �� Value��%#  &	       &	 q    'o� �    'm� �    �� .R�� .+�A� $ outerCircle��$��10��$��10������ $	 0,0&�$	 0,1&�.��� $ 1Q�$ Black��� $ 1Q�$ Black��A� $��8��$��8������ $	 0,0&�$	 0,1&�.��� $ 1Q� ��  �� *TemplatedParent+$ Background.Color�� $ 1Q� ��  �� *TemplatedParent+$ Background.Color�(� .���� $��20q�#��z���� .R�� .+�� $��10��$��10�� �   ��� @KL ��� ���� $	 0,0&�$	 0,1&�.��� $ 1Q� ��  �� *TemplatedParent+$ Background.Color�� $ 1Q� ��  �� *TemplatedParent+$ Background.Color�(� .���� $��20q�#��z���� "	 o�.��� 
 � ��$ 15(��� "	 m�.��� 
 � ��$ 15(��� 
 � ��
 ��ح��� 
 � ��
 �   � � ColumnDefinitions �� $��10 �$	��3.7���� $
��0.6* �$
��1330��$
��1330���� $��10 ��� $
��0.2* ��� $��10 �$��10��.+�� -   $��gridonlineGraphic��$��1�� � RowDefinitions �� $��35 ��� $��* ��� $��30 ��� $��30 ��� $��10 ��� $��25 ��� $��25 ��� $��5 ��� $��25 ��� $��5 ��� $��5* �.+��� -   $��grboxMechTehProperties��$"��Mehaničko-tehničke osobine��$��18(�
����  �
����  �$��2��$��0��$��5��$��3��o� $
��Auto��$��24��$��1��$��0��$	��Top=�$
��Left=�.@�m� -   $��menuIOnlineMain��$7��1Opcije podešavanja Jungovog modula elastičnosti��$��2q�$y�Visible=�8  2Microsoft.Research.DynamicDataDisplay.ChartPlotter  -     Name$ plotter��
�������$��1��$��1��$	��750��$	��750��$	��Top=�3  -Microsoft.Research.DynamicDataDisplay.Plotter   Children. =  7Microsoft.Research.DynamicDataDisplay.MarkerPointsGraph  -   $��_MarkerGraph��  -   $��_MarkerGraph2��  -   $��_MarkerGraph3��  -	   $��_MarkerGraph4��  -
   $��_MarkerGraph5��  -   $��_MarkerGraph6��  -   $��_MarkerGraph7��  -   $��_MarkerGraph8��  -   $��_MarkerGraph9��$y�	Collapsed=�  -   $��_MarkerGraph10��2  ,Microsoft.Research.DynamicDataDisplay.Header  $��Times New Roman)�$��Grafik��=  7Microsoft.Research.DynamicDataDisplay.VerticalAxisTitle  $��Times New Roman)�$��R [MPa]��?  9Microsoft.Research.DynamicDataDisplay.HorizontalAxisTitle  $��Times New Roman)�$��e [%]��� -   $��gridRRelReHRp02��$��3��$��1�� �� $��10 �$��10���� $
��2.5* ��� $��10 �$��10���� $
��2.5* ��� $��10 �$��10���� $
��2.5* ��� $��10 �$��10��.+�� -   $��	gridRpmin��$��1�� �� $��30 �$��30���� $��30 �$��30���� $��30 �$��30���� $
��0.3* ��� $��50 �$��50��.+��� -   $��lblFm��$��2��$��Center=�$��Right=�$
��Wrap=�$
��Fm  ���� -   $
��tfFm��$��3��  textboxStyle1#��[  �� 
IsReadOnly . $��Bottom=��� -   $��lblFmEnd��$��4��$��Center=�$��Center=�$
��Wrap=�$	��[N]��� -   $��gridReH��$��3�� �� $��30 �$��30���� $��30 �$��30���� $��30 �$��30���� $
��0.3* ��� $��50 �$��50��.+��� -   $��chbReHVisibility��$��Center=�$��Center=��� -   $��stylePointReH��$��1��$��10��$��10��#��[ 
��������� -   $��lblReH��$��2��$��Center=�$��Right=�$
��Wrap=�$��13(�$��ReH  ���� -   $��tfReH��$��3��#��[  . $��Bottom=��� -   $��	lblReHEnd��$��4��$��Center=�$��Center=�$
��Wrap=�$��[MPa]��� -   $��gridRp02��$��30��$��5�� �� $��30 �$��30���� $��30 �$��30���� $��30 �$��30���� $
��0.3* ��� $��50 �$��50��.+��� -   $��chbRp02Visibility��$��Center=�$��Center=��� -   $��stylePointRp02��$��1��$��10��$��10��#��[ 
�������� -   $��lblRp02��$��2��$��Right=�$��Center=�$
��Wrap=�$��13(�$
��Rp02���� -   $��tfRp02��$��3��#��[  . $��Stretch=�$��Center=��� -    $��
lblRp02End��$��4��$��Center=�$��Center=�$
��Wrap=�$��[MPa]��� -!   $��gridRRpmaxFmax��$��5��$��1�� �� $��10 �$��10���� $
��2.5* ��� $��10 �$��10���� $
��2.5* ��� $��10 �$��10���� $
��2.5* ��� $��10 �$��10��.+�� -"   $��	gridRpmax��$��1�� �� $��30 �$��30���� $��30 �$��30���� $��30 �$��30���� $
��0.3* ��� $��50 �$��50��.+��� -#   $��chbRmVisibility��$��Center=�$��Center=��� -$   $��stylePointRm��$��1��$��10��$��10��#��[ 
���   ��� -%   $��lblRm��$��2��$��Right=�$��Center=�$
��Wrap=�$
��Rm  ���� -&   $
��tfRm��$��3��#��[  . $��Stretch=�$��Center=��� -'   $��lblRmEnd��$��4��$��Center=�$��Center=�$
��Wrap=�$��[MPa]��� -(   $��gridFmax��$��3�� �� $��30 �$��30���� $��30 �$��30���� $��30 �$��30���� $
��0.3* ��� $��50 �$��50��.+��� -)   $��chbReLVisibility��$��Center=�$��Center=��� -*   $��stylePointRel��$��1��$��10��$��10��#��[ 
��� ����� -+   $��lblReL��$��2��$��Right=�$��Center=�$
��Wrap=�$��ReL  ���� -,   $��tfReL��$��3��#��[  . $��Stretch=�$��Center=��� --   $��lblRpminEnd��$��4��$��Center=�$��Center=�$
��Wrap=�$��[MPa]��� -.   $��gridA��$��5�� �� $��30 �$��30���� $��30 �$��30���� $��30 �$��30���� $
��0.3* ��� $��50 �$��50��.+��� -/   $��chbAVisibility��$��Center=�$��Center=��� -0   $��stylePointA��$��1��$��10��$��10��#��[ 
���  ���� -1   $
��lblA��$��2��$��Center=�$��Right=�$
��Wrap=�$	��A  ���� -2   $	��tfA��$��3��#��[  . $��Bottom=��� -3   $��lblAEnd��$��4��$��Center=�$��Center=�$
��Wrap=�$	��[%]���� $��1��$��8��$��Center=�$��Center=�  Flat3DBorderBrush#��[ $��5q����#BBB._��� -4   $��btnPlottingMode��$��8��$��1��$	��270��$��30��$��Center=�$��Center=����� $��Center=�$��Center=�$
��Wrap=�
��� � �$��18(�$
��Bold"�$��1. UČITAJ IZABRANI FAJL���� -5   $��lblOnlineStatus��$��Offline mod��$��Center=�$��Center=�$��0��$��1��$��20(��� -6   $ ��chbTurnOnFittingCorrection��$!��Ukljuci korekciju fitovanja��$��8��$��16��$
��Left=�$��	727,5,0,0q�$	��Top=�� -7   $��gridofflineoptions��$��3�� �� $��10 �$��10���� $	��520 � �� $��10 �$��10���� $��35 ��� $��10 ��� $��26 ��� $��26 ��� $��45 ��� $��26 ��� $��26 ��� $��45 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��30 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 �.+��� -8   $��grboxRazmeraPlotting��$��Razmera��$��18(�
����  �
����  �$��7��$��0��$��6��$��2���� -9   $��grboxFilePathPlotting��$��Putanja fajla��$��18(�
����  �
����  �$��13��$��0��$��5��$��3���� -:   $��grboxFitting��$��	Fitovanje��$��18(�
����  �
����  �$��18��$��0��$��13��$��3��� -;   $ ��gridAutomaticGoToPrintMode��$��1��$��3�� �� $��30 �$��30���� $��4* ��� $��10 �$��10���� $��30 �$��30���� $��10 �$��10��.+��� -<   $��chbAutomaticGoToPrintMode��$��1��$��0��$
��Left=�$��Center=�$1��+Ukljuci automatsko prebacivanje u print mod��� -=   $��gridShowChangeOfRAndE��$��1��$��4�� �� $��30 �$��30���� $��4* ��� $��10 �$��10���� $��30 �$��30���� $��10 �$��10��.+��� ->   $��chbShowChangeOfRAndEe��$��1��$��0��$
��Left=�$��Center=�{�.  $y�Hidden=�$G��APrikaži grafike promene napona i izduženja prilikom učitavanja���� -?   $��lblGraphicRatio��$��8��$��1��$��20(�$
��Bold"�$��Center=�$��Center=�$
��Wrap=�$��   Razmera   ��� -@   $��gridRbtnsRation��$��9��$��1�� �� $��10 �$��10���� $��2* ��� $��10 �$��10���� $
��1.6* ��� $��10 �$��10��.+�	� -A   $��rbtnAutoOff�� 	� 	GroupName$ razmeraOffline��$��1��$��16��$
��Left=�$y�Hidden=�$��Center=����� $��Center=�$��Center=�$��ukljuci zumiranje��	� -B   $��
rbtnManual��$ razmeraOffline��$��16��$y�Hidden=�$
��Left=�$��3��$��Center=����� $��Center=�$��Center=�$��iskljuci zumiranje��� -C   $��gridChboxsRation��$��10��$��1�� �� $��10 �$��10���� $��2* ��� $��10 �$��10���� $
��1.6* ��� $��10 �$��10��.+��� -D   $*��$chbPrikaziOriginalAfterRatioChanging��$��1��$��0��$
��Left=�$��Center=�$y�Hidden=�$.��(Prikaži grafik sa originalnim podacima ���� -E   $*��$chbPrikaziFitovaniAfterRatioChanging��$��3��$��0��$
��Left=�$��Center=�$y�Hidden=�$��Prikaži fitovani grafik ��� -F   $��gridTfPreassure��$��10��$��1�� �� $��10 �$��10���� $
��1.5* ��� $��10 �$��10���� $
��1.5* ��� $��10 �$��10��.+��� -G   $��lblRatioPreassure��$��1��$��Right=�$��Center=�$��Napon[MPa]  :  ���� -H   $��tfRatioForce��$��3��#��[ $
��Left=�$
��Auto��$��25��$	��Top=�� -I   $��gridTfElongation��$��11��$��1�� �� $��10 �$��10���� $
��1.5* ��� $��10 �$��10���� $
��1.5* ��� $��10 �$��10��.+��� -J   $��lblRatioElongation��$��1��$��Right=�$��Center=�$��Izduženje[%]  :  ���� -K   $��tfRatioElongation��$��3��#��[ $
��Left=�$��25��$
��Auto��� -L   $��gridPlottingFilePath��$��1��$��15�� �� $��10 �$��10���� $
��Auto ��� $��10 �$��10���� $��30 �$��30���� $��10 �$��10��.+��� -M   $��lblFilepath��$��1��$��0��$��Center=�$A��;Putanja fajla koju želite da prikažete u offline modu :  ���� $��3��$��Center=�$��Center=�#��[ $��5q����#BBB._��� -N   $��btnChooseDatabasePath��$	��...��$��20��$��30��$��20(�$
��Bold"�$��Center=�$��Center=�$��3��� -O   $!��gridPlottingFilePathTextBox��$��1��$��16�� �� $	��17* ��� $��9* � �� $��10 �$��10���� $	��541 �$	��700���� $
��159* �.+��� -P   $��tfFilepathPlotting��$��1��#��[ $
��Left=�$��25��$	��500��$��2��� -Q   $��gridFititngModeCheckBox��$��1��$��19�� �� $��30 �$��30���� $��4* ��� $��10 �$��10���� $��30 �$��30���� $��10 �$��10��.+��� -R   $��chbFittingMode��$��1��$��0��$
��Left=�$��Center=�$��Ukljuci fitovanje��� -S   $#��gridFititngManualModeCheckBox��$��1��$��20�� �� $��30 �$��30���� $��4* ��� $��10 �$��10���� $��30 �$��30���� $��10 �$��10��.+��� -T   $��chbFititngManualMode��$��1��$��0��$
��Left=�$��Center=�$!��Ukljuci rucni mod fitovanja��� -U   $��gridFititngModePointAuto��$��1��$��21�� �� $��30 �$��30���� $��4* ��� $��10 �$��10���� $��30 �$��30���� $��10 �$��10��.+��� -V   $��fittingAutoPoint��$��1��$��0��$
��Left=�$��Center=�$��Tačka Rm-a je :  ��� -W   $%��gridFititngModePointAutoContent��$��1��$��22�� �� $��30 �$��30���� $��* ��� $��30 �$��30���� $
��0.5* ��� $��30 �$��30��.+�� -X   $*��$gridInnerFititngModePointAutoContent��$��1�� �� $
��Auto ��� $��30 �$��30���� $
��Auto ��� $��45 �$��45��.+��� -Y   $��tffittingCrossheadPointX��$��0��#��[ $
��Left=�$��Center=� . �� -Z   $��lblpr��$��1��$��Center=�$��Center=�$	��[%]���� -[   $��tffittingCrossheadPointY��$��2��#��[ $
��Left=�$��Center=� . �� -\   $��lblmpa��$��3��$��Center=�$��Center=�$��[MPa]��� -]   $��
gridpoint1��$��23��$��1�� �� $��30 �$��30���� $
��Auto ��� $��10 �$��10���� $
��1.5* ��� $��10 �$��10��.+��� -^   $��stylePoint1��$��0��$��10��$��10��#��[ 
����  �� -_   $��gridManualPoints��$��3�� �� $��20 �$��20���� $
��Auto ��� $��30 �$��30���� $
��Auto ��� $��45 �$��45��.+��� -`   $��lblordnum1man��$��0��$
��Left=�$��Center=�$��T1���� -a   $��tffittingManPoint1X��$��1��#��[ $
��Left=�$��Bottom=�$��0,0,0,2q��� -b   $��lblprman��$��2��$��Center=�$��Center=�$	��[%]���� -c   $��tffittingManPoint1Y��$��3��#��[ $
��Left=�$��Center=��� -d   $��	lblmpaman��$��4��$��Center=�$��Center=�$��[MPa]��� -e   $��
gridpoint2��$��24��$��1�� �� $��30 �$��30���� $
��Auto ��� $��10 �$��10���� $
��1.5* ��� $��10 �$��10��.+��� -f   $��stylePoint2��$��0��$��10��$��10��#��[ 
��� ���� -g   $��gridManualPoints2��$��3�� �� $��20 �$��20���� $
��Auto ��� $��30 �$��30���� $
��Auto ��� $��45 �$��45��.+��� -h   $��lblordnum2man��$��0��$
��Left=�$��Center=�$��T2���� -i   $��tffittingManPoint2X��$��1��#��[ $
��Left=�$��Center=��� -j   $��	lblprman2��$��2��$��Center=�$��Center=�$	��[%]���� -k   $��tffittingManPoint2Y��$��3��#��[ $
��Left=�$��Center=��� -l   $��
lblmpaman2��$��4��$��Center=�$��Center=�$��[MPa]��� -m   $��
gridpoint3��$��25��$��1�� �� $��30 �$��30���� $
��Auto ��� $��10 �$��10���� $
��1.5* ��� $��10 �$��10��.+��� -n   $��stylePoint3��$��0��$��10��$��10��#��[ 
���  ��� -o   $��gridManualPoints3��$��3�� �� $��20 �$��20���� $
��Auto ��� $��30 �$��30���� $
��Auto ��� $��45 �$��45��.+��� -p   $��lblordnum3man��$��0��$
��Left=�$��Center=�$��T3���� -q   $��tffittingManPoint3X��$��1��#��[ $
��Left=�$��Center=��� -r   $��	lblprman3��$��2��$��Center=�$��Center=�$	��[%]���� -s   $��tffittingManPoint3Y��$��3��#��[ $
��Left=�$��Center=��� -t   $��
lblmpaman3��$��4��$��Center=�$��Center=�$��[MPa]��� -u   $��gridpointR2R3R4Border��$��26��$��1�� �� $��30 �$��30���� $
��Auto ��� $��10 �$��10���� $
��1.5* ��� $��10 �$��10��.+��� -v   $��stylePoint4��$��0��$��10��$��10��#��[ 
���   �� -w   $��gridManualPoints4��$��3�� �� $��40 �$��40���� $
��Auto ��� $��30 �$��30���� $
��Auto ��� $��45 �$��45��.+��� -x   $��lblR2R3R4Border��$��0��$
��Left=�$��Center=�$��R2/R4���� -y   $��tfR2R4ManualOfflineMode��$��1��#��[ $
��Left=�$��Center=��� -z   $��	lblprman4��$��2��$��Center=�$��Center=�$	��[%]��� -{   $��gridFittingButtons��$��28��$��1�� �� $��2* �$	��210���� $��10 ��� $��* �$��10���� $��10 ��� $��* �.+��� $��0��$��Center=�$��Center=�#��[ $��5q����#BBB._��� -|   $��btnTurnOnFitting��$	��350��$y�	Collapsed=�$��30��$��18(�$
��Bold"�
��� � �$!��2. PRIKAŽI FITOVANI GRAFIK��$��Center=�$��Center=�$��0��� -}   $!��gridTotalRelativeElongation��$��30��$��1�� �� $��8 �$��8���� $��* ��� $��* ��� $��33 �.+��� -~   $��btnCalculateTotalRelElong��$y�Hidden=�$��Stretch=�$��Center=�
��� � �$/��)3. Izračunaj ukupno relativno izduženje��$��0��$��2��� -   $$��gridLBLTotalRelativeElongation��$��31��$��1�� �� $��8 �$��8���� $��* �.+��� -�   $��lblCalculateTotalRelElong��$y�Hidden=�$
��Left=�$��Center=�$,��&Nije izračunato relativno izduženje.��$��1���� $��1��$��29��$��Center=�$��Center=�#��[ $��5q����#BBB._��� -�   $��btnShowOfflineOptions��$	��250��$��30��$��18(�$
��Bold"�$��OPCIJE OFFLINE MODA��$��1��$��33��$��Center=�$��Center=�