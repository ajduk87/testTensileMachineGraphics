   M S B A M L   `   `   `  ����   testTensileMachineGraphics,    &testTensileMachineGraphics.PrintScreen   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Y UDynamicDataDisplay, Version=0.3.0.0, Culture=neutral, PublicKeyToken=5b7d744a7263923f=d34http://research.microsoft.com/DynamicDataDisplay/1.0  .���    	Gradient5#��[    � ColumnDefinitions  �� $��10 �$��10���� $
��2.1* �$
��1330��$
��1330���� $��10 ��� $��* �$	��800���� $��10 �$��10�� � 	Resources   buttonTemplate A�Name �� 
StartPoint �� EndPoint � Offset � Color �� RelativeSource �� Path  buttonTemplateTriangle	 � Points��%�  &	       &	 q    �� .R�� .+�A� $ outerCircle��$��10��$��10������ $	 0,0&�$	 0,1&�.��� $ 1Q�$ Black��� $ 1Q�$ Black��A� $��8��$��8������ $	 0,0&�$	 0,1&�.��� $ 1Q� ��  �� *TemplatedParent+$ Background.Color�� $ 1Q� ��  �� *TemplatedParent+$ Background.Color�(� .���� $��20q�#��z���� .R�� .+�� $��10��$��10��	 �   ��� @KL ��� ���� $	 0,0&�$	 0,1&�.��� $ 1Q� ��  �� *TemplatedParent+$ Background.Color�� $ 1Q� ��  �� *TemplatedParent+$ Background.Color�(� .���� $��20q�#��z��.+�� -   $��gridPrintScreenGraphic��$��1��
 � RowDefinitions
 �� $��70 ��� $	��750 ��� $��15 ��� $��30 ��� $��10 ��� $��25 ��� $��10 ��� $��35 ��� $��10 ��� $��25 �.+�� -   $��
gridLegend��  �� $��10 �$��10���� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��10 �$��10��.+�� -   $��
rp02Legend��$��1��$��90��  �� $��* ��� $��2* �.+��� -   $��stylePointRp02��$��0��$��10��$��10��#��[ 
������$��Center=�$��Center=��� -   $��lblRp02��$��1��$
��Left=�$��Center=�$
��Wrap=�$��18(�$
��Rp02��� -   $��
rt05Legend��$��2��$��90��  �� $��* ��� $��2* �.+��� -   $��stylePointRt05��$��0��$��10��$��10��#��[ 
���� ��$��Center=�$��Center=��� -   $��lblRt05��$��1��$
��Left=�$��Center=�$
��Wrap=�$��18(�$
��Rt05��� -	   $��	reLLegend��$��3��$��90��  �� $��* ��� $��2* �.+��� -
   $��stylePointReL��$��0��$��10��$��10��#��[ 
��� ���$��Center=�$��Center=��� -   $��lblReL��$��1��$
��Left=�$��Center=�$
��Wrap=�$��18(�$	��ReL��� -   $��	reHLegend��$��4��$��90��  �� $��* ��� $��2* �.+��� -   $��stylePointReH��$��0��$��10��$��10��#��[ 
�������$��Center=�$��Center=��� -   $��lblReH��$��1��$
��Left=�$��Center=�$
��Wrap=�$��18(�$	��ReH��� -   $��rmLegend��$��5��$��90��  �� $��* ��� $��2* �.+��� -   $��stylePointRm��$��0��$��10��$��10��#��[ 
���   �$��Center=�$��Center=��� -   $��lblRm��$��1��$
��Left=�$��Center=�$
��Wrap=�$��18(�$��Rm��� -   $��agLegend��$��6��$��90��  �� $��* ��� $��2* �.+��� -   $��stylePointA��$��0��$��10��$��10��#��[ 
���  ��$��Center=�$��Center=��� -   $
��lblA��$��1��$
��Left=�$��Center=�$
��Wrap=�$��18(�$��A��8  2Microsoft.Research.DynamicDataDisplay.ChartPlotter  -     Name$ plotterPrint��$
��Left=�$��1��$��1��
�������$	��750��$	��750��$
��1330��$
��1330��$	��Top=�3  -Microsoft.Research.DynamicDataDisplay.Plotter   Children. =  7Microsoft.Research.DynamicDataDisplay.MarkerPointsGraph  -   $��_MarkerGraph��  -   $��_MarkerGraph2��  -   $��_MarkerGraph3��  -   $��_MarkerGraph4��  -   $��_MarkerGraph5��  -   $��_MarkerGraph6��  -   $��_MarkerGraph7��  -   $��_MarkerGraph8��  -   $��_MarkerGraph9��  -   $��_MarkerGraph10��  -    $��_MarkerGraph11��  -!   $��_MarkerGraph12��  -"   $��_MarkerGraph13��  -#   $��_MarkerGraph14��  -$   $��_MarkerGraph15��2  ,Microsoft.Research.DynamicDataDisplay.Header  $��Times New Roman)�$�� ��=  7Microsoft.Research.DynamicDataDisplay.VerticalAxisTitle  $��Times New Roman)�$��R [MPa]��?  9Microsoft.Research.DynamicDataDisplay.HorizontalAxisTitle  $��Times New Roman)�$��e [%]��� -%   $��gridButtons��$��1��$��3��  �� $	��260 ��� $��10 ��� $��30 ��� $��10 �$��10���� $	��155 ��� $��10 ��� $	��280 ��� $��10 ��� $��30 ��� $��10 �$��10��.+��� $��0��$��Center=�$��Center=�  Flat3DBorderBrush#��[ $��5q����#BBB._��� -&   $��btnLoadSampleReport��$	��250��$��30��$��18(�$
��Bold"�$��0��$��UČITAJ IZVEŠTAJ��$��Center=�$
��Left=��� $��2��$��Center=�$��Center=�#��[ $��5q����#BBB._��� -'   $��btnChooseSampleReportPath��$	��...��$��20��$��30��$��20(�$
��Bold"�$��Center=�$��Center=�$��2���� $��6��$��Center=�$��Center=�#��[ $��5q����#BBB._��� -(   $��btnLoadSumReport��$	��270��$��30��$��18(�$
��Bold"�$��6��$��PRIKAŽI ZBIRNI IZVEŠTAJ��$��Center=�$
��Left=��� $��8��$��Center=�$��Center=�#��[ $��5q����#BBB._��� -)   $��btnChooseSumReportPath��$	��...��$��20��$��30��$��20(�$
��Bold"�$��Center=�$��Center=�$��8��� -*   $��gridTextboxes��$��1��$��5��  �� $	��460 ��� $��1 ��� $	��455 ��� $��1 ��� $	��300 �.+��� -+   $��tfFilepathSampleReport��$��0��  textboxStyle1#��[ $
��Left=�$��25��$	��445���� -,   $��tfFilepathSumReport��$��2��#��[ $
��Left=�$��25��$	��445���� --   $��cmbInputWindow��$��12��$	��215��$��25��$
��Left=�$��15(�.@��� $��15(�.��OPŠTI PODACI�� $��15(�.��USLOVI ISPITIVANJA�� $��15(�.��	MATERIJAL�� $��15(�.��POLOŽAJ EPRUVETE�� $��15(�.��
EPRUVETA�� $��15(�.��
NAPOMENA� -.   $��gridForPrintButtons��$��7��  �� $	��220 ��� $��1 ��� $��1 ��� $��10 �$��10���� $	��215 ��� $��10 ��� $	��165 ��� $��5 ��� $	��230 �$	��230���� $��60 ��� $	��210 ��� $
��135* �.+��� $��0��$��Center=�$��Center=�#��[ $��5q����#BBB._��� -/   $"��btnPrintSampleOnlyMakeReport��$	��220��$��30��$��18(�$
��Bold"�$��0��$��NAPRAVI IZVEŠTAJ��$��Center=�$
��Left=��� $��4��$��12��$��Center=�$��Center=�#��[ $��5q����#BBB._��� -0   $��btnPrintSample��$	��200��$��30��$��18(�$
��Bold"�$��4��$��12��$��ŠTAMPAJ��$��Center=�$
��Left=��� $��6��$��10��$��Center=�$��Center=�#��[ $��5q����#BBB._��� -1   $��btnSaveSamplePrintMode��$	��150��$��30��$��18(�$
��Bold"�$��6��$��10��$��ZAPAMTI��$��Center=�$
��Left=��� $��8��$��Right=�$��Center=�#��[ $��5q����#BBB._��� -2   $ ��btnEnterNewSamplePrintMode��$	��180��$��30��$��18(�$
��Bold"�$��8��$��NOVO MERENJE��$��Center=�$
��Left=��� $��10��$��Center=�$��Center=�#��[ $��5q����#BBB._��� -3   $��btnSampleDataPrintMode��$	��200��$��30��$��18(�$
��Bold"�$��PODACI O UZORKU��$��Center=�$
��lEFT=�$��10���� -4   $��chbShowReports��$�� �� � ToolTip$E ?Prikaži izveštaje prilikom klika na dugme sa natpisom ZAPAMTI��$��7��$��19��$
��Left=�$��4,10,0,0q�$	��Top=�$��20��$��2���� -5   $��chbPrikaziOpcije��$�� ��$ Prikazi opcije fitovanja��$��8��$��16��$
��Left=�$��	22,10,0,0q�$	��Top=��� $��11��$��Center=�$��Center=�#��[ $��5q����#BBB._��� -6   $��btnFooterOptions��$	��180��$��30��$��18(�$
��Bold"�$��Opcije obrasca��$��Center=�$
��lEFT=�$��10��� -7   $��gridForPrintButtons2��$��9��  �� $	��400 ��� $��10 ��� $��10 ��� $��10 �$��10���� $��35 ��� $��10 ��� $	��165 ��� $��5 ��� $	��180 �$	��180���� $��10 ��� $	��210 �.+��� -8   $��chbChangeOfRAndE��$-��'PRIKAŽI PROMENE R-a I e-a U IZVEŠTAJU��$��18(�$
��Bold"�$��Center=�$
��Left=�$��Center=�$��Center=�$��0���� $��6��$��4��$��Center=�$��Center=�#��[ $��5q����#BBB._��� -9   $��btnGoToOnlineMode��$	��350��$��30��$��18(�$
��Bold"�$��6��$��5��$��POTVRDI UNETE PODATKE��$��Center=�$
��Left=��� -:   $��chbCalculateNManual��$��RUČNO n��$��18(�$
��Bold"�$��Center=�$��Center=�$��Center=�$��Center=�$��10��� -;   $��gridPrintScreenoptions��$��3��  �� $��10 �$��10���� $��* ��� $��10 �$��10��
 �� $��10 �$��10���� $��35 ��� $��10 ��� $��26 ��� $��26 ��� $��45 ��� $��26 ��� $��26 ��� $��45 ��� $��26 ��� $��26 ��� $��45 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��45 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 �