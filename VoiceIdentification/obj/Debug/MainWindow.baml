   M S B A M L   `   `   `  ����N  JVoiceIdentification, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:VoiceIdentificationVoiceIdentification  5   	   $    VoiceIdentification.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:VoiceIdentification     5   	     � Title$  
MainWindow��5   	   $	��350��6   $	��525��6)   .��5	      � .+�5
   
   �� -   5         ��Name$ 	recordBtn��5
      $��Record��6$   $
��Left=�65   $��	47,66,0,0q�6P   $	��Top=�6c   $��75��6{   6�   �� 5   
   -   5        $ identifyBtn��5      $��Identify��6&   $
��Left=�69   $��
228,66,0,0q�6T   $	��Top=�6h   $��75��6�   {�.  6�   �� 5   
   -   5         ��Name$" _identificationResultStckPnl��5       �� Orientation$ Vertical=�6;   $��Center=�6R   $��Center=�6m   $y�Hidden=�6�   $��	6,62,6,20q�6�   $	��272��6�   $��50��6�   .+�5      �� $ 
Horizontal=�6   $��30��62   .+�5      �� $��
5, 5, 0, 0q�6   Identified speaker:6E   �� 5      -   5         ��Name$! _identificationResultTxtBlk��5      $��
5, 5, 0, 0q�6A   $
��Bold"�6U   5      �� 5      $ 
Horizontal=�6   .+�5      �� $��
5, 5, 0, 0q�6   Confidence:6=   �� 5      -   5        $% _identificationConfidenceTxtBlk��5      $��
5, 5, 0, 0q�6E   $
��Bold"�6Y   5      5      5      5      