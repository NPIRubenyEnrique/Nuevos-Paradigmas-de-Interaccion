   M S B A M L   `   `   `  ����  SkeletonBasics-WPF8    2Microsoft.Samples.Kinect.SkeletonBasics.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Skeleton Basics��5   	   $	��735��6!   $	��770��6.    � 	Resources 5         MediumGreyBrush �� Color  KinectPurpleBrush  KinectBlueBrush �� 
TargetType �� Property �� Value  SquareCheckBox �� 
TargetType �� Orientation �Name	 ��Name
 ��Name �� TextAlignment �� Triggers P� Property P� Value �� 
TargetName��%1  &	        &	 #     &	 F     '�� i     &	 �     7  7  �� 5   
   $ 	#ff6e6e6e��62   �� 5   
   $ 	#ff52318f��64   �� 5	   
   $ 	#ff00BCF2��62   �� 5
   
   " ��6   .�5      ��  �@J�SnapsToDevicePixels . 64   5      �� 5   
   " ��6   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      � .+�5      �� $ 
Horizontal=�6)   
������ .+�5   "   � $ SquareCheckBoxChecked��6'   .+�5   &   �� $	 CheckedNormal��6,   $��Images\CheckedNormal.png��6C   $
��None=�6e   $��Center=�6t   �� 5   &   $	 CheckedHover��6,   $��Images\CheckedHover.png��6B   $
��None=�6c   $��Center=�6r   $y�	Collapsed=�6�   5   #   � 5   "   $ SquareCheckBoxUnchecked��6'   $y�	Collapsed=�6H   .+�5   &   �� $	 UncheckedNormal��6,   $ ��Images\UncheckedNormal.png��6E   $
��None=�6i   $��Center=�6x   �� 5   &   $	 UncheckedHover��6,   $��Images\UncheckedHover.png��6D   $
��None=�6g   $��Center=�6v   $y�	Collapsed=�6�   5   #   �� 5   "   $
 SquareCheckBoxText��6,   #��z��6H   $
 Left=�6i   $��Center=�6   8��  $��15(�6�   $��9,0,0,0q�6�   5      5       5      P� 5       �@d�	IsChecked$ falseY�6;   . �5    "   �� $ SquareCheckBoxChecked��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5!   "   $ SquareCheckBoxUnchecked��6O    � y�$ Visible=�6?   6O   5"      P� 5#       �@J�IsMouseOver . . �5$   "   �� $ CheckedNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5%   "   $ UncheckedNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5&   "   $ CheckedHover��6O    � y�$ Visible=�6?   6O   �� 5'   "   $ UncheckedHover��6O    � y�$ Visible=�6?   6O   �� 5(   "   8  $ SquareCheckBoxText��6h    � ��5)      5+      5-      5.      5/      .��51      � -   5        $��
layoutGrid��51      $��	10 0 10 0q�6    � RowDefinitions 52   
   �� 53      $
��Auto �6   �� 54      $��* �6   �� 55      $
��Auto �6   �� 56      $
��Auto �6   .+�58   
   ]� $��0��6   $��0 0 0 20q�6!   .+�59      �� $
��Left=�6   $��Images\Logo.png��6*   $
��Fill=�6C   $��32��6R   $��81��6^   $��0 10 0 5q�6i   �� 5:      $��Right=�6   $��0 0 -1 0q�6/   $��Bottom=�6A   #��[  6\   $��Segoe UI)�6�   $��18(�6�   $
��Pong��6�   �� 5;      $��1��6   $��Images\Status.png��6$   $
��None=�6?   $��Center=�6N   $��0 0 0 5q�6k   5<      (� 5=   
   $��1��6    (� Stretch$ Uniform=�6   $��Center=�61   .��5>      �� -   5        $��Image��5>      $	��640��6!   $	��480��6-   5?      �� 5@   
   -   5        $��checkBoxSeatedMode��5@   �   $��2��6   #��[ 6    $��Seated Mode��6H   $
��Auto��6^   $
��Left=�6l   $��Center=�6�   $��
0 10 10 10q�6�   6�   �� 5A   
   -   5        $��	statusBar��5A   ?   $��3��6   $��Stretch=�6!   $��Bottom=�6P   
�������#��[  6~   .@�5B      �� $��0 0 0 10q�6   .��5C      �� -   5        $��statusBarText��5C      $��-1 0 0 0q�61   20Click 'Seated' to change skeletal pipeline type!6u   5D      5E      �� 5F   
   -   5         ��Name$ puntuacionUno��5F      $��0��6'   $��85��63   $��
-325,0,0,0q�6?   $	��Top=�6S   $��85(�6k   $��OCR A Extended)�6y   
���   �$��Center=�6�   $	��110��6�   $��	ExtraBold"�6�   $y�Hidden=�6�   $��1��6�   �� 5G   
   -   5        $ puntuacionDos��5G      $��0��6'   $��85��63   $��	325,0,0,0q�6?   $	��Top=�6R   $��85(�6j   $��OCR A Extended)�6x   
���   �$��Center=�6�   $	��110��6�   $��	ExtraBold"�6�   $y�Hidden=�6�   $��1��6�   �� 5H   
   -	   5        $
 pong��5H      $
��PONG��6   $��OCR A Extended)�6-   $��50��6I   $��0,0,0,0q�6U   $	��Top=�6f   $��40(�6~   
���   �$��Center=�6�   $	��110��6�   $
��Bold"�6�   $y�Visible=�6�   $��1��6�   �� 5I   
   -
   5        $
 menu��5I      $��MENU DE INTRODUCCION��6   $��OCR A Extended)�6=   $��46��6Y   $��0,80,0,0q�6e   $	��Top=�6w   $��40(�6�   
���   �$��Center=�6�   $	��492��6�   $
��Bold"�6�   $y�Hidden=�6�   $��1��6�   �� 5J   
   -   5        $ 
Inicial1_1��5J      $5��/LA BOLA NEGRA REPRESENTA LA POSICION DE TU MANO��6$   $��OCR A Extended)�6^   $��35��6z   $��	0,160,0,0q�6�   $	��Top=�6�   $��25(�6�   
���   �$��Center=�6�   $	��719��6�   $
��Bold"�6�   $y�Hidden=�6  $��1��6!  �� 5K   
   -   5        $ 
Inicial1_2��5K      $4��.PARA PULSAR UN BOTON MANTENER LA MANO SOBRE EL��6$   $��OCR A Extended)�6]   $��34��6y   $��	0,200,0,0q�6�   $	��Top=�6�   $��25(�6�   
���   �$��Center=�6�   $	��704��6�   $
��Bold"�6�   $y�Hidden=�6  $��1��6   �� 5L   
   -   5        $ 
Inicial2_1��5L      $9��3CONTROLA LA POSICION DE LA PALA CON TU MANO DERECHA��6$   $��OCR A Extended)�6b   $��34��6~   $��	0,160,0,0q�6�   $	��Top=�6�   $��20(�6�   
���   �$��Center=�6�   $	��688��6�   $
��Bold"�6�   $y�Hidden=�6  $��1��6%  �� 5M   
   -   5        $ 
Inicial2_2��5M      $=��7LA ALTURA DE LA PALA SE CORRESPONDERA CON LA DE TU MANO��6$   $��OCR A Extended)�6f   $��34��6�   $��	0,200,0,0q�6�   $	��Top=�6�   $��20(�6�   
���   �$��Center=�6�   $	��704��6�   $
��Bold"�6  $y�Hidden=�6  $��1��6)  �� 5N   
   -   5        $ 
Inicial3_1��5N      $*��$TAMBIEN PUEDE JUGAR UNA SOLA PERSONA��6$   $��OCR A Extended)�6S   $��34��6o   $��	0,200,0,0q�6{   $	��Top=�6�   $��20(�6�   
���   �$��Center=�6�   $	��444��6�   $
��Bold"�6�   $y�Hidden=�6  $��1��6  �� 5O   
   -   5        $ 
Inicial3_2��5O      $?��9MOVIENDOSE A DERECHA E IZQUIERDA PARA CONTROLAR CADA PALA��6$   $��OCR A Extended)�6h   $��34��6�   $��19,240,27,0q�6�   $	��Top=�6�   $��20(�6�   
���   �$��Center=�6�   $	��696��6�   $
��Bold"�6  $y�Hidden=�6  $��1��6-  �� 5P   
   -   5        $ 
Inicial3_3��5P      $>��8PARA VOLVER A MOSTRAR ESTA AYUDA PONER LA MANO SOBRE '?'��6$   $��OCR A Extended)�6g   $��34��6�   $��	0,300,0,0q�6�   $	��Top=�6�   $��20(�6�   
���   �$��Center=�6�   $	��704��6�   $
��Bold"�6  $y�Hidden=�6  $��1��6*  �� 5Q   
   -   5        $ 	siguiente��5Q      $��	SIGUIENTE��6#   $��OCR A Extended)�67   $��38��6S   $��300,170,0,0q�6_   $��Center=�6t   $��25(�6�   
���   �$��Center=�6�   $	��148��6�   $
��Bold"�6�   $y�Hidden=�6�   $��1��6�   �� 5R   
   -   5        $ anterior��5R      $��ANTERIOR��6"   $��OCR A Extended)�65   $��38��6Q   $��-300,170,0,0q�6]   $��Center=�6s   $��25(�6�   
���   �$��Center=�6�   $	��136��6�   $
��Bold"�6�   $y�Hidden=�6�   $��1��6�   �� 5S   
   -   5        $ pausa1��5S      $��PAUSAR��6    $��OCR A Extended)�61   $��25��6M   $��-550,-420,0,0q�6Y   $��Center=�6p   $��18(�6�   
���   �$��Center=�6�   $��75��6�   $
��Bold"�6�   $y�Hidden=�6�   $��1��6�   �� 5T   
   -   5        $ pausa2��5T      $��PAUSAR��6    $��OCR A Extended)�61   $��25��6M   $��550,-420,0,0q�6Y   $��Center=�6o   $��18(�6�   
���   �$��Center=�6�   $��75��6�   $
��Bold"�6�   $y�Hidden=�6�   $��1��6�   �� 5U   
   -   5        $ 	reiniciar��5U      $��	REINICIAR��6#   $��OCR A Extended)�67   $��25��6S   $��
0,-420,0,0q�6_   $��Center=�6s   $��18(�6�   
���   �$��Center=�6�   $	��108��6�   $
��Bold"�6�   $y�Hidden=�6�   $��1��6�   �� 5V   
   -   5        $ ayuda��5V      $��?��6   $��MS Reference Sans Serif)�6+   $��40��6P   $��-500,450,0,0q�6\   $��Center=�6r   $��30(�6�   
���   �$��Center=�6�   $��30��6�   $
��Bold"�6�   $y�Hidden=�6�   $��1��6�   �� 5W   
   -   5        $ volver��5W      $��¿Volver al tutorial?��6    $��MS Reference Sans Serif)�6?   $��30��6d   $��-250,460,0,0q�6p   $��Center=�6�   $��20(�6�   
���   �$��Center=�6�   $	��205��6�   $
��Bold"�6�   $y�Hidden=�6�   $��1��6  5X      5Y      