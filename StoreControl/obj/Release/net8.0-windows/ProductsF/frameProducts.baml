   M S B A M L   `   `   `  ���� G  CStoreControl, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null+clr-namespace:StoreControlStoreControl       StoreControl.frameProducts   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     ,localclr-namespace:StoreControl     -     N� Title$  frameProducts�� � 	Resources  	  goldC �� Color 	 grayC  	darkGrayC  WatermarkTextBlock  WatermarkTextBox �� 
TargetType �� Property �� Value �� 	Resources  Key j� 
AlignmentX j� 
AlignmentY	 j� Stretch
 !� Visual �� Source �� Path �� Triggers �� Binding �� RelativeSource �� Value  CueBannerBrush �� 
TargetType ��Name �� Triggers P� Property P� Value �� 
TargetName �� CornerRadius&   System.Windows.Controls.DataGrid=  7System.Windows.Controls.Primitives.DataGridColumnHeader ]� Thumb  ComboBoxTemplate ��Name ��Name �� IsDropDownOpen � AllowsTransparency � ActualWidth J� SnapsToDevicePixels K� IsItemsHost-  'System.Windows.Input.KeyboardNavigation    DirectionalNavigation! d�Name" d� 	IsChecked# �� Mode$ F�Name  CustomComboBoxItem% �� Padding��%�  &	        &	      &	 *     &	 ?     &	 ^     7  7  7  7  '�� �    7  7  7  7   7  '�� -    7  7  7   7  7   7  '�� �    7  '      7  7  7  7  '  y    7  7  7  '�� �    7  7  7   &	 �    7  7  7  7  7  7  7  7   &	 �    7  7   7   7  �� $ 	#FFFFD700���� $ 	#FFDDDDDD���� $ 	#FF2C2C2C���� $��Suchwort��
���   ��� WatermarkTextBox��" ��.���  � ��$ 14(���  � ��$ 0,0,0,0q���  � ��$ 5q���  � ��8   ��  � ��8   ��!� CueBannerBrush $
 Left=�$ Center=�$
	 None=�
 �� 8�� $��4,0,0,0q����� 8  $
 Text� ��  �� *Text+ �� *Self+  .a���  � ��# �  �� " ��.���  � ��8   ��  � ��8  ��  � ��$ 14(���  � ��$ Segoe UI)���  � ��$ 5q���  � ��$ 1q���  � ��8  ��  � �� �� " ��.R��� $��border��#��z��#��z��#��z��._��� $ PART_ContentHost��$��0q� P�  �@J�	IsFocused$
 True��. ��� $ border��8   � ��P�  � {�$ False��. ��� 8   � ���� " ��.���  � ��
 �������  � ��8   ��  � ��$ 1q���  � ��8  ��  � ��$ 8q���  � �� �� " ��.R��� #��z��$ 4��#��z��#��z��._��� $��Center=�$��Center=�$
��True�� P�  �@J�IsMouseOver$
 True��. ��� 8   � ����  � ��
 �   ��� 8   � ��P�  � ��$
 True��. ��� 8   � ���� 8   � ���� " ��.���  � ��8   ��  � ��$ 14(���  � ��$ Segoe UI)���  � ��$
 Bold"��� "  .���  � ��8   ��  � ��8  ��  � ��$ Segoe UI)���  � ��$ 13(���  �@ RowBackground �	#FF3A3A3A��   �@ AlternatingRowBackground8  ��  �@ GridLinesVisibility$
 None=���  �@ HeadersVisibility$ Column=���  � ��8  ��  � ��$ 1q���  �@ RowHeaderWidth$ 0Q��� "  .���  � ��8   ��  � ��8  ��  � ��$
 Bold"���  � ��$ 0,0,0,1q���  � ��8  �� " ��.���  � ��8   ��  � ��8  ��  � ��$ 10����  � �� �� " ��.R�� #��z��.+�]� $��
PART_Track��$
��true�� n� 8�� �� " ��.���  � ��$ Segoe UI)���  � ��$ 14(���  � ��$ 5q���  � ��$ 1q� P�  �@��
IsEditable$
 True��. ���  � �� �� " ��.R�� .+��� $ PART_EditableTextBox��8��  8�� $��14(�$��Segoe UI)�$��5q�$��1q�8�� $��Center=�.������ " ��.R��� $ border��#��z��#��z��#��z��._��� $ PART_ContentHost��$��0q� P�  �@J�	IsFocused$
 True��. ��� $ border�� � ��
 � ���P�  � {�$ False��. ��� 8   � ��.��� $��Popup��$��Bottom=�#��z $
 True��$|�False��$��Slide=�.���� 8�� 8�� $��1q�#��z ._��� $��4,6,4,6q�$
 True��.���� $
 True��$  	Contained=�d� $! ToggleButton��$��30��$��30��$��Right=�$��Center=�
������ 
������ $��1q�" �� *IsDropDownOpen+$# TwoWay=� �� *TemplatedParent+��F� $$ 	ArrowPath��8�� "����        � 	=  	= � z     	$��12��$��8��$��Center=�$��Center=����� " d�.���  � ��
 ����  P�  �@J�IsMouseOver$
 True��P�  �@d�	IsChecked$
 TrueY�. ���  � ��8  �� " ��.���  � ��
 �������  � ��$ 14(���  � ��$
 Bold"���  � ��$ Segoe UI)���  � ��8   ��  � �� �� " ��.R��� $ Bd��#��z��$% 4q�._���  P�  �@J�IsMouseOver$
 True��. ��� $ Bd��8   � ����  � ��
 �   �P�  �@��
IsSelected$
 True��. ��� $ Bd��8   � ��P�  � {�$ False��. ��� 8   � ��.��� -   & �Name$& gridFP�����	#FF2C2C2C.+��� -   ' ��Name$' 	productId��$��	productId��$
��Left=�$��9,14,0,0q�$	��Top=�( J� RenderTransformOrigin$( -0.349,-0.503&�$��31��$��79��$��14(��� -   $ productIdTB��$
��Left=�$��
131,14,0,0q�) �� 
IsReadOnly$
) True��* �� TextWrapping$
* Wrap=�$	��Top=�$	��131��$��30��$��14(����#C4C4C4���#000000�� -   $' productName��$��productName��$
��Left=�$��9,64,0,0q�$	��Top=�$( -0.349,-0.503&�$��31��$	��104��$��14(��� -   $ productNameTB��$
��Left=�$��
131,64,0,0q�$
* Wrap=�$	��Top=�$	��290��$��30��$��14(��� -   $' description��$��description��$
��Left=�$��	9,128,0,0q�$	��Top=�$( -0.349,-0.503&�$��31��$	��104��$��14(��� -   $ descriptionTB��$
��Left=�$��131,101,0,0q�$
* Wrap=�$	��Top=�$	��289��$��79��$��14(��� -	   $' category��$��category��$
��Left=�$��
450,14,0,0q�$	��Top=�$( -0.349,-0.503&�$��31��$	��104��$��14(��� -
   + ��Name$+ 
categoryCB��, �� 
IsEditable$
, True��- �� DisplayMemberPath$- categoryName��. �� SelectedValuePath$. categoriesId��$
��Left=�$��
574,14,0,0q�$	��Top=�$	��131��$��30��$��14(�$( 
0.962,0.74&�#��[ ���� / �� ResourceKey$/ CustomComboBoxItem���� -   $' articleNumber��$��articleNumber��$
��Left=�$��
450,64,0,0q�$	��Top=�$( -0.349,-0.503&�$��31��$	��118��$��14(��� -   $ articleNumberTB��$
��Left=�$��
574,64,0,0q�$
* Wrap=�$	��Top=�$	��131��$��30��$��14(��� -   $' purchasePrice��$��purchasePrice��$
��Left=�$��
730,13,0,0q�$	��Top=�$( -0.349,-0.503&�$��31��$	��104��$��14(��� -   $ purchasePriceTB��$
��Left=�$��
855,13,0,0q�$
* Wrap=�$	��Top=�$	��131��$��30��$��14(��� -   $' sellingPrice��$��sellingPrice��$
��Left=�$��
730,64,0,0q�$	��Top=��� -   $ sellingPriceTB��$
��Left=�$��
855,64,0,0q�$
* Wrap=�$	��Top=�$	��131��$��30��$��14(��� -   $' quantity��$��quantity��$
��Left=�$��450,114,0,0q�$	��Top=�$( -0.349,-0.503&�$��31��$	��104��$��14(��� -   $ 
quantityTB��$
��Left=�$��574,114,0,0q�$
* Wrap=�$	��Top=�$	��131��$��30��$��14(��� -   $' minimumStock��$��minimumStock��$
��Left=�$��730,114,0,0q�$	��Top=�$( -0.349,-0.503&�$��38��$	��120��$��14(��� -   $ minimumStockTB��$
��Left=�$��855,114,0,0q�$
* Wrap=�$	��Top=�$	��131��$��30��$��14(��� -   0 ��Name$	0 img��$	��Top=�$
��Left=�$��1049,50,0,0q�$	��300��$
��Fill=�  -   1  Name$1 dataGrid��$2   HorizontalScrollBarVisibility$
2 Auto=�3   
IsReadOnly$
3 True��$
��Left=�4   AutoGenerateColumns$4 False��$��27��$��	7,369,0,9q�$( 0.5,0.5&�5   SelectionMode$5 Single=�z�V� .~��� �� �� 6 �� Angle$6 0.188Q�T� �� -   7 ��Name$
7 newB��$��Button��$
��Left=�$��
68,219,0,0q�$	��Top=�$��49��$	��115���� -   $7 editB��$��Button��$
��Left=�$��268,219,0,0q�$	��Top=�$��49��$	��115���� -   $7 deleteB��$��Button��$
��Left=�$��468,219,0,0q�$	��Top=�$��49��$	��115���� -   $7 clearB��$��Button��$
��Left=�$��668,219,0,0q�$	��Top=�$��49��$	��115���� -   $7 minimumStockB��$��Button��$
��Left=�$��868,219,0,0q�$	��Top=�$��49��$	��115���� -   $
7 imgB��$��Button��$
��Left=�$��1048,12,0,0q�$	��Top=�$��97��$��25���� -   $ searchTB��$
��Left=�$��363,300,0,0q�$
* Wrap=�$	��Top=�$	��322��$��29��#��[ �� -   8 ��Name$8 
NoDataText��$��No data available��$��Center=�$��Center=�$��16(�
�������$y�	Collapsed=��� -   $7 
xSearching��$��X��$
��Left=�$��644,301,0,0q�$	��Top=�$��27��$��40��$��0,0,0,0q�