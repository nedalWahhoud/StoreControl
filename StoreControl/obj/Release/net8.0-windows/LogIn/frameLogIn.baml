   M S B A M L   `   `   `  ���� G  CStoreControl, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null7 clr-namespace:StoreControl.LogInStoreControl.LogIn  #    StoreControl.LogIn.frameLogIn   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     2local clr-namespace:StoreControl.LogIn       N� Title$  
frameLogIn�� � 	Resources  	  goldC �� Color 	 grayC  	darkGrayC �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Triggers	 P� Property
 P� Value  ComboBoxTemplate �� Triggers ��Name ��Name ��Name �� 
TargetName �� IsDropDownOpen � AllowsTransparency � ActualWidth J� SnapsToDevicePixels K� IsItemsHost-  'System.Windows.Input.KeyboardNavigation   DirectionalNavigation d�Name  CustomComboBoxItem �� Padding��%�  &	        &	      &	 *     '�� ?     7  7  7   7  7   7  '�� �    7  &	 .    7  7  7  7  7  7  &	     7  7   7   7  'G� �    7  7  7  7   7  �� $ 	#FFFFD700���� $ 	#FFDDDDDD���� $ 	#FF2C2C2C���� " ��.���  � ��
 �������  � ��8   ��  � ��$ 1q���  � ��8  ��  � ��$ 8q���  � �� �� " ��.R��� #��z��$ 4��#��z��#��z��._��� $��Center=�$��Center=�$
��True�� P� 	 �@J�IsMouseOver$

 True��. ��� 8   � ����  � ��
 �   ��� 8   � ��P� 	 � ��$

 True��. ��� 8   � ���� 8   � ���� " ��.���  � ��8   ��  � ��$ 14(���  � ��$ Segoe UI)���  � ��$
 Bold"��� " ��.���  � ��$ Segoe UI)���  � ��$ 14(���  � ��$ 5q���  � ��$ 1q� P� 	 �@��
IsEditable$

 True��. ���  � �� �� " ��.R�� .+��� $ PART_EditableTextBox��8��  8�� $��14(�$��Segoe UI)�$��5q�$��1q�8�� $��Center=�.������ " ��.R��� $ border��#��z��#��z��#��z��._��� $ PART_ContentHost��$��0q� P� 	 �@J�	IsFocused$

 True��. ��� $ border�� � ��
 � ���P� 	 � {�$
 False��. ��� 8   � ��.��� $��Popup��$��Bottom=�#��z $
 True��$|�False��$��Slide=�.���� 8�� 8�� $��1q�#��z ._��� $��4,6,4,6q�$
 True��.���� $
 True��$ 	Contained=�d� $ ToggleButton��$��0��$��Right=��� " ��.���  � ��
 �������  � ��$ 14(���  � ��$
 Bold"���  � ��$ Segoe UI)���  � ��8   ��  � �� �� " ��.R��� $ Bd��#��z��$ 4q�._���  P� 	 �@J�IsMouseOver$

 True��. ��� $ Bd��8   � ����  � ��
 �   �P� 	 �@��
IsSelected$

 True��. ��� $ Bd��8   � ��P� 	 � {�$
 False��. ��� 8   � ���� " G�.���  � ��8   ��  � ��8  ��  � ��$ 14(���  � ��$ Segoe UI)���  � ��$ 5q���  � ��$ 1q���  � ��8  ��  � �� �� " G�.R��� $��border��#��z��#��z��#��z��._��� $ PART_ContentHost��$��0q� P� 	 �@J�	IsFocused$

 True��. ��� $ border��8   � ��P� 	 � {�$
 False��. ��� 8   � ��.��� -    �Name$ gridLI�����	#FF2C2C2C.+��� -    ��Name$ userName��$��userName��$��Center=�$��Center=�$��-200,-100,0,0q��� -   $��
userNameCB��$��Center=� �� 
IsEditable$
 True�� �� StaysOpenOnEdit$
 True�� �� IsTextSearchEnabled$ False��$��Center=�$��120,-100,0,0q�$	��200��$��30�� �� DisplayMemberPath$ userName��   TabNavigation$
 Once=�#��[  �� SelectedItem �� *SelectedUser+����   �� ResourceKey$  CustomComboBoxItem���� -   $ passwort��$��passwort��$��Center=�$��Center=�$��
-200,0,0,0q�G� -   $��
passwordTB��$��Center=�$��Center=�$��	120,0,0,0q�$	��200��$��30���� -   ! ��Name$! logInB��$��logIn��$��Center=�$��Center=�$��120,100,0,0q�$��49��$	��115��