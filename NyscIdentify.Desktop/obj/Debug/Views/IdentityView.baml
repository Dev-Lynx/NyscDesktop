   M S B A M L   `   `   `  ����O  KNyscIdentify.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:NyscIdentify.Desktop.ViewsNyscIdentify.Desktop.Views  5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      R NSystem.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089>.clr-namespace:System.Data;assembly=System.DataSystem.Data 5	      Q-clr-namespace:NyscIdentify.Desktop.ViewModelsNyscIdentify.Desktop.ViewModels  5
      ] YNyscIdentify.Common.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null�\clr-namespace:NyscIdentify.Common.Infrastructure;assembly=NyscIdentify.Common.Infrastructure"NyscIdentify.Common.Infrastructure 5      �oclr-namespace:NyscIdentify.Common.Infrastructure.Resources.Controls;assembly=NyscIdentify.Common.Infrastructure5NyscIdentify.Common.Infrastructure.Resources.Controls 5      �yclr-namespace:NyscIdentify.Common.Infrastructure.Resources.AttachedProperties;assembly=NyscIdentify.Common.Infrastructure?NyscIdentify.Common.Infrastructure.Resources.AttachedProperties 5      �pclr-namespace:NyscIdentify.Common.Infrastructure.Resources.Behaviors;assembly=NyscIdentify.Common.Infrastructure6NyscIdentify.Common.Infrastructure.Resources.Behaviors 5      -    'NyscIdentify.Desktop.Views.IdentityView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:NyscIdentify.Desktop.Views     5      X TPrism.Unity.Wpf, Version=7.1.0.431, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59R	 NPrism.Wpf, Version=7.1.0.431, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59&prismhttp://prismlibrary.com/  	 5      2sys&clr-namespace:System;assembly=mscorlib  	 5      8s.clr-namespace:System.Data;assembly=System.Data  	 5	      8vm-clr-namespace:NyscIdentify.Desktop.ViewModels  	 5
      V
 RMaterialDesignThemes.Wpf, Version=2.5.1.1345, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes 
 5      gcore\clr-namespace:NyscIdentify.Common.Infrastructure;assembly=NyscIdentify.Common.Infrastructure 
 5      �coreControlsoclr-namespace:NyscIdentify.Common.Infrastructure.Resources.Controls;assembly=NyscIdentify.Common.Infrastructure 
 5      �capyclr-namespace:NyscIdentify.Common.Infrastructure.Resources.AttachedProperties;assembly=NyscIdentify.Common.Infrastructure 
 5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      h dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bei9http://schemas.microsoft.com/expression/2010/interactions  5      �coreBehaviorspclr-namespace:NyscIdentify.Common.Infrastructure.Resources.Behaviors;assembly=NyscIdentify.Common.Infrastructure  5      -   5        $��ParentControl��5       ! 	 Prism.Mvvm.ViewModelLocator    AutoWireViewModel  . $��20q�5   <    � 	Resources 5         OpenSearchBox �� TargetProperty �� 
TargetName9  3System.Windows.Media.Animation.EasingDoubleKeyFrame O� KeyTime O� Value  CloseSearchBox  OpenFilterBox  CloseFilterBox  FadeInFilterOverlay8  2System.Windows.Media.Animation.EasingColorKeyFrame �� KeyTime �� Value R� KeyTime  System.Windows.Visibility	   Visible
 R� Value  FadeOutFilterOverlay   Hidden  OpenSortBox  CloseSortBox  FadeInSortOverlay 	 FadeOutSortOverlay��%	  &	        &	 �     &	 <    &	 �    &	 t    &	 �    &	 �    &	 �    &	 (    &		 c    �� 5   
   .��5      U� $! (cap:Directions.Horizontal)�6,   $ 	SearchBox��6d   .\�5        $ 0:0:0.2��6'   $ 0Q�69   5      5      �� 5   
   .��5       U� $! (cap:Directions.Horizontal)�6,   $ 	SearchBox��6d   .\�5!        $ 0:0:0.2��6'   $ 1Q�69   5"      5#      �� 5'   
   .��5(      U� $ (cap:Directions.Vertical)�6,   $ 	FilterBox��6b   .\�5)        $ 0:0:0.3��6'   $ 0Q�69   5*      5+      �� 5-   
   .��5.      U� $ (cap:Directions.Vertical)�6,   $ 	FilterBox��6b   .\�5/        $ 0:0:0.3��6'   $ 1Q�69   50      51      �� 53   
   .��54      �� $0 *(Panel.Background).(SolidColorBrush.Color)�6+   $ FilterBoxOverlay��6r   .g�55        $ 0:0:0.5��6&   $ #000000��68   56      T� 57      $ (UIElement.Visibility)�6,   $ FilterBoxOverlay��6_   ..�58      i� $ 0:0:0.1��6)   #
 Z	 6;   59      5:      �� 5<   
   .��5=      �� $0 *(Panel.Background).(SolidColorBrush.Color)�6+   $ FilterBoxOverlay��6r   .g�5>        $ 0:0:0.5��6&   $ 	#00000000��68   5?      T� 5A      $ (UIElement.Visibility)�6,   $ FilterBoxOverlay��6_   ..�5B      i� $ 0:0:0.6��6)   #
 Z 6;   5C      5D      �� 5H   
   .��5I      U� $ (cap:Directions.Vertical)�6,   $ SortBox��6b   .\�5J        $ 0:0:0.3��6'   $ 0Q�69   5K      5L      �� 5N   
   .��5O      U� $ (cap:Directions.Vertical)�6,   $ SortBox��6b   .\�5P        $ 0:0:0.3��6'   $ 1Q�69   5Q      5R      �� 5T   
   .��5U      �� $0 *(Panel.Background).(SolidColorBrush.Color)�6+   $ SortBoxOverlay��6r   .g�5V        $ 0:0:0.5��6&   $ #000000��68   5W      T� 5X      $ (UIElement.Visibility)�6,   $ SortBoxOverlay��6_   ..�5Y      i� $ 0:0:0.1��6)   #
 Z	 6;   5Z      5[      �� 5]   
   .��5^      �� $0 *(Panel.Background).(SolidColorBrush.Color)�6+   $ SortBoxOverlay��6r   .g�5_        $ 0:0:0.5��6&   $ 	#00000000��68   5`      T� 5b      $ (UIElement.Visibility)�6,   $ SortBoxOverlay��6_   ..�5c      i� $ 0:0:0.6��6)   #
 Z 6;   5d      5e      5h      .��5i      �� ���#383838$��.5q�5j       �� CornerRadius$ 5 0 0 5��6"   ._�5k   
   �  � RowDefinitions 5l      �� 5m      $
��Auto �6    �� 5n      $��* �6    �� 5o      $
��Auto �6    .+�5r      ]� ��.  $
��10 5q�6.    5s       �� MergedDictionaries �� Source�� 5t       5u      �� 5v      $K E/NyscIdentify.Common.Infrastructure;component/Resources/Material.xaml?�61   %    5x      .+�5{      �� -   5        $��LayoutToggle��5}       
 MaterialDesignFlatButton#��[
 5{      $	��3 0q�5|      $��Right=�6'   $��Center=�5}   -   $��26��6H   $��26��6S   $	��5 0q�5~      ��6&   �� *ChangeLayoutCommand+ � ToolTip 5      �� *Layout+  EnumToStringConverter �� 	Converter# [ .��5�      � .+�5�      ' 
 !MaterialDesignThemes.Wpf.PackIcon  � 
 �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.1.1345, Culture=neutral, PublicKeyToken=null]]   Kind$ ViewList=�62   $
��Auto��6B   $��25��6O   y�5�   2   �� *Layout+  EnumToBoolConverter# [ /  )NyscIdentify.Common.Infrastructure.Layout   List �� ConverterParameter# Z   5�      $ ViewGrid=�62   $
��Auto��6B   $��25��6O   y�5�   1   �� *Layout+# [    Grid# Z 5�      5�      d� 5�      -   5        $��FilterToggle��5�       ) %MaterialDesignFlatPrimaryToggleButton#��[ 5�      $��Right=�5�   3   $	��3 0q�6J   $��Center=�5�      $��26��6:   $��26��6E   $	��5 0q�5�      .��5�        $ Filter=�6.   $
��Auto��6<   $��25��6I   5�      d� 5�      -   5        $��
SortToggle��5�      #��[ 5�      $��Right=�5�   1   $	��3 0q�6H   $��Center=�5�      $��26��6:   $��26��6E   $	��5 0q�5�      .��5�        $
 Sort=�6.   $
��Auto��6:   $��25��6G   5�      � 5�      }�. $��Right=�6+   $	��200��6B   $	��300��6Q   $	��5 0q�6`   .+�5�      �� -   5        $��	SearchBox��5�        HalfRoundedTextBox#���  6/   P  JNyscIdentify.Common.Infrastructure.Resources.AttachedProperties.Directions   
Horizontal$ 1Q�5�      $��Center=�6<   $��Stretch=�5�       J� RenderTransformOrigin$ 0.5,0.5&�6<   $��25��5�      $��Center=�6*   $��1 1 0 1q�5�      ��68   �� *Online.SearchQuery+ �� UpdateSourceTrigger$ PropertyChanged=� �� Delay$
 1000��.��6�   z�5�      V� 5�      .~�5�   "   �� �� 5�   "   �� 5�   "   T� 5�   "    T� X 5�   &   `� 5�   *     MathConverter `� 	Converter# [ 67    `� ConverterParameter$	 x*y��6b   .1�5�   .   ��  �� ElementName$ 	SearchBox��66    �� Path$ ActualWidth�6N   �� 5�   .   $ 	SearchBox��66   $! (cap:Directions.Horizontal)�6N   5�   +   5�   #   5�      .��5�   3   .	  (System.Windows.Interactivity.Interaction 	  Triggers 5�      9
  3Microsoft.Expression.Interactivity.Core.DataTrigger
  5�        
  Value$
  True��6j   D  >Microsoft.Expression.Interactivity.Core.PropertyChangedTrigger!   Binding! 6-   �� $ SearchToggle��$ 	IsChecked�.  (System.Windows.Interactivity.TriggerBase"   Actions." 5�   "   F  @Microsoft.Expression.Interactivity.Media.ControlStoryboardAction  ?  9Microsoft.Expression.Interactivity.Media.StoryboardAction#   
Storyboard## [ 6=   $   ControlStoryboardOption$
$ Stop=�6j     5�   "   ## [  6=   $
$ Play=�6i   5�      
  5�      $  False��6j   ! 6-   �� $ SearchToggle��$ 	IsChecked�." 5�   "     ## [  6=   $
$ Stop=�6i     5�   "   ## [ 6=   $
$ Play=�6j   B  <Microsoft.Expression.Interactivity.Core.ChangePropertyAction  5�   "   %   PropertyName$
% Text��6:   &   Value$&  ��6N   5�      .��5�   2   5�      �� 5�        	MainBrush#���  6   ' �� Padding$' 0 0 1 0q�6F   $��Center=�5�      $��Right=�68   $ 	13 0 0 13��6T   ._�5�      d� -   5        $��SearchToggle��5�      #��[ 5�   '   $	��3 0q�5�   -   $��26��5�      $��26��6$   .��5�        $ Magnify=�66   $
��Auto��6E   $��25��6R   5�      5�      5�      5�      � 5�      $��1��6   }�. .+�5�      �� ( J� 	AllowDrop( . ��6,   �� *Online.Data+y�5�      �� *Layout+# [ # Z ) �� SelectedItem) 5�      �� *SelectedIdentity+$ PropertyChanged=�* �� Mode$* TwoWay=�+ 	  	Behaviors+ 5�      T  NNyscIdentify.Common.Infrastructure.Resources.Behaviors.ItemDoubleClickBehavior  5�      , J� InputBindings, 5�      d� 5�      - d� MouseAction$- LeftDoubleClicke�6'   . �� Command. 6E   �� *ViewIdentityCommand+ 5�      �� 5�       5�      �� 5�   "   $K E/NyscIdentify.Common.Infrastructure;component/Resources/Material.xaml?�65   %    5�      ��5�      �� 5�      / �� 
TargetType"/ ��6      MaterialDesignGridViewItem0 �� BasedOn#0 [ 6:   .�5�      �� 1 �� Property1 � ��  MainForegroundBrush2 �� Value#2 �  6;   �� 5�      1 �@�ContextMenu  IdentityItemMenu#2 �  6<   3 �� Triggers3 5�      P� 5�   "   4 P� Property4 �@��
IsSelected5 P� Value5 . . �5�   &   �� 1 � ��  AuxForegroundBrush#2 �  6C   �� 5�   &   1 � ��  AuxBrush#2 �  6C   5�   #   5�      ��5�      �� 5�      .R�5�      � $��2q�6#   , 5�   "   d� 5�   &   $- LeftDoubleClicke�63   . 6Q   �� *!DataContext.ViewIdentityCommand+6 �� RelativeSource6 �� 7 �� AncestorType"7 ��.+�5�   "   �� ��6,   �� *Name+5      5     8 �� View8 5     �� 5     .P�5     �� $��File Number��6-   9 �� DisplayMemberBinding9 6B   �� *FileNo+�� 5     $��	Last Name��6-   9 6@   �� *
LastName+�� 5     $��Other Names��6-   9 6B   �� *
OtherNames+�� 5	     $��Gender��6-   9 6=   �� *Gender+�� 5
     $��State Of Origin��6-   9 6F   �� *StateOfOrigin+�� 5     $
��Rank��6-   9 6;   �� *Rank+�� 5     $��
Department��6-   9 6A   �� *
Department+5     5     �� 5     -   5        $��IdentityListBox��5  O   ( . $��Disabled=�5     $��Stretch=�5     ��5  +   �� *Online.Data+) 5     �� *SelectedIdentity+$ PropertyChanged=�$* TwoWay=�y�5     �� *Layout+# [ # Z , 5     d� 5     $- LeftDoubleClicke�6'   . 6E   �� *ViewIdentityCommand++ 5       5      5     �� 5       5!     �� 5"  "   $K E/NyscIdentify.Common.Infrastructure;component/Resources/Material.xaml?�65   %    5$     ��5'     �� 5(     "/ ��6      MaterialDesignListBoxItem#0 [ 69   .�5)     �� 1 � ��#2 �  6;   �� 5*     1 �@�ContextMenu#2 �  6<   3 5.     P� 5/  "   4 �@��
IsSelected5 . . �50  &   �� 1 � ��#2 �  6C   �� 51  &   1 � ��#2 �  6C   52  #   54     ��57     �� 58     .R�59     � : K� IsItemsHost: . ; � Orientation$; 
Horizontal=�6;   $��10q�5:  (   ��5;  (   �� *ActualWidth+6 �� "7 ��< �� Mode$< FindAncestor=�5=     ��5@     �� 5A     .R�5B     �� , 5C  "   d� 5D  &   $- LeftDoubleClicke�64   . 6R   �� *!DataContext.ViewIdentityCommand+6 �� "7 ��._�5G  "   � $��5q�6'    5H  &   �� 5I  *   $	��120 �68   �� 5J  *   $��40 �68   .+�5M  &   �� $	��100��6-   = J� Effect= 5N  *   3  -System.Windows.Media.Effects.DropShadowEffect  5O  .   >   
BlurRadius$> 5Q�6?   ?   Color$? #000000��6N   @   	Direction$	@ 325Q�6^   A   ShadowDepth$A 1Q�6n   B   Opacity$B .8Q�6~   ._�5R  *   J  DNyscIdentify.Common.Infrastructure.Resources.Controls.ResourceViewer  C   UsePlaceholderC . D   HighPriorityD .  E   ResourceE 6r   �� *
Passport+5S  '   �� 5V  &   $��1��6y   $	��100��6�   $
��Wrap=�5W  ,   $��0 5 0 0q�6@   F �� TextAlignment$F Center=�6Q   ��5V  0   �� *
FullName+  TitleCaseConverter# [ 5W  Q   5X  #   5Y     5Z     5\     �� 5^     -   5        $��FilterBoxOverlay��5^     ���	#00FFFFFF$y�Hidden=�6H   G J� Opacity$G .3Q�6\    5_     /  )System.Windows.Interactivity.EventTrigger  5`     H   	EventName$H MouseUp��6)   ." 5a       8  2System.Windows.Interactivity.TargetedTriggerActionI   
TargetName$I FilterToggle��66   $% 	IsChecked��6P   $& False��6i   - 	 'Prism.Interactivity.InvokeCommandAction  5b     J   CommandJ 68   �� *UpdateTableCommand+5c     5e     �� 5g     -	   5        $��	FilterBox��5g     $��Center=�6*   $��Bottom=�6G   $ 0.5,0.5&�6b   K   Vertical$K 1Q�5h     $	��600��65   $	��300��6D   $	��500��6T    5i     �� 5j     $K E/NyscIdentify.Common.Infrastructure;component/Resources/Material.xaml?�6-   %     5m     
  5n     $
  True��6f   ! 6)   �� $ FilterToggle��$ 	IsChecked�." 5o       $
$ Stop=�69   ## [ 6X     5p     $
$ Stop=�69   ## [ 6X     5q     $
$ Play=�69   ## [ 6X     5r     $
$ Play=�69   ## [ 6X     5s     $I 
SortToggle��66   $% 	IsChecked��6N   $& False��6g   5t     
  5u     $  False��6f   ! 6)   �� $ FilterToggle��$ 	IsChecked�." 5v       $
$ Stop=�69   ## [ 6X     5w     $
$ Stop=�69   ## [ 6X     5x     $
$ Play=�69   ## [ 6X     5y     $
$ Play=�69   ## [ 6X   5z     z�5}     V� 5~     .~�5     �� �� 5�     �� 5�     T� 5�     L T� YL 5�  "   `� 5�  &   # [ 63   $ (x*y)��6^   .1�5�  *   �� $ 	FilterBox��62   $ ActualHeight�6J   �� 5�  *   $ 	FilterBox��62   $ (cap:Directions.Vertical)�6J   5�  '   5�     5�     ._�5�     # 
 MaterialDesignThemes.Wpf.Card  $��30 10q�6*   .��5�     �  5�     �� 5�  "   $
��Auto �60   �� 5�  "   $��* �60   �� 5�  "   $
��Auto �60   .+�5�     �� $��Center=�6(   $��Filter��6C   $��50(�6Q     RobotoLight#���  5�  $   $
��Thin"�5�  $   $
��0 10q�66   �� 5�     $��1��6+   $
��Auto=�68   $��Disabled=�5�  +   .��5�  "   �� ��6/   �� *Online.Filters+��5�  &   �� 5�  *   .R�5�  .   � $	��0 4q�63   M � ColumnDefinitionsM 5�  2   �� 5�  6   $��* �6G   �� 5�  6   $��* �6G   �� 5�  6   $��* �6G   �� 5�  6   $
��Auto �6G   .+�5�  2   �� N �� DisplayMemberPath$
N Name��5�  \   ) 
 #MaterialDesignThemes.Wpf.HintAssistO   Hint$O 	Filter By��5�  9   P   
IsFloatingP . $
��10 0q�5�  9   $��Center=�6G   ��5�  ;   �� $ ParentControl��$- 'DataContext.Online.FilterableProperties�Q �� SelectedValueQ 5�  9   �� *
Property+5�  G   �� 5�  2   $��1��6;   $O Filter��5�  9   P . $
��10 0q�5�  9   $��Center=�6G   ��5�  K   �� $ ParentControl��$( "DataContext.Online.FilterOperators�Q 5�  9   �� *FilterOperator+��5�  6   �� 5�  :   .R�5�  >   �� R �� Padding$	R 1 0q�6H   ��6V   �� # [ 5�  ;   5�  3   �� 5�  2   $��2��5�  2   $O Value��5�  2   P . $��Center=�5�  2   $
��10 0q�6M   ��5�  :   �� *FilterValue+$* TwoWay=�$ PropertyChanged=�5�  M   �� 5�  2   $��3��69   $	��5 0q�5�  N   $	��2 0q�5�  1   $��Bottom=�6?   $��18��6Z   ��5�  1   �� $ ParentControl��$% DataContext.RemoveFilterCommand���5�  1   �� .��5�  6     $ Close=�6N   5�  3   5�  /   5�  +   5�  #   5�     �� 5�      & "MaterialDesignFloatingActionButton#��[ 6%   $��2��5�  !   $��Right=�5�     $��Center=�69   $��	0 20 0 10q�6T   ��5�  .   �� *AddFilterCommand+.��5�  "     $
 Plus=�6:   5�     5�     5�     5�     �� 5�     -
   5        $��SortBoxOverlay��5�     ���	#00FFFFFF$y�Hidden=�6F   $G .3Q�6Z    5�       5�     $H MouseUp��6)   ." 5�       $I 
SortToggle��66   $% 	IsChecked��6N   $& False��6g     5�     J 68   �� *UpdateTableCommand+5�     5�     �� 5�     -   5        $��SortBox��5�     $��Center=�6(   $��Bottom=�6E   $ 0.5,0.5&�6`   $K 1Q�5�     $	��600��65   $	��300��6D   $	��500��6T    5�     �� 5�     $K E/NyscIdentify.Common.Infrastructure;component/Resources/Material.xaml?�6-   %     5�     
  5�     $
  True��6d   ! 6)   �� $ 
SortToggle��$ 	IsChecked�." 5�       $
$ Stop=�69   ## [ 6X     5�     $
$ Stop=�69   ## [	 6X     5�     $
$ Play=�69   ## [ 6X     5�     $
$ Play=�69   ## [ 6X     5�     $I FilterToggle��66   $% 	IsChecked��6P   $& False��6i   5�     
  5�     $  False��6d   ! 6)   �� $ 
SortToggle��$ 	IsChecked�." 5�       $
$ Stop=�69   ## [ 6X     5�     $
$ Stop=�69   ## [ 6X     5�     $
$ Play=�69   ## [ 6X     5�     $
$ Play=�69   ## [	 6X   5�     z�5�     V� 5�     .~�5�     �� �� 5�     �� 5�     T� 5�     L 5�  "   `� 5�  &   # [ 63   $ (x*y)��6^   .1�5�  *   �� $ SortBox��62   $ ActualHeight�6H   �� 5   *   $ SortBox��62   $ (cap:Directions.Vertical)�6H   5  '   5     5     ._�5       $��30 10q�6*   .��5     �  5	     �� 5
  "   $
��Auto �60   �� 5  "   $��* �60   �� 5  "   $
��Auto �60   .+�5     �� $��Center=�6(   $
��Sort��6C   $��50(�6O   #���  5  $   $
��Thin"�5  $   $
��0 10q�66   �� 5     $��1��6+   $
��Auto=�68   $��Disabled=�5  +   .��5  "   �� ��6/   �� *Online.Sorts+��5  &   �� 5  *   .R�5  .   � $	��0 4q�63   M 5  2   �� 5  6   $��* �6G   �� 5  6   $��* �6G   �� 5  6   $
��Auto �6G   .+�5  2   �� $
N Name��5!  \   $O Sort By��5"  9   P . $
��10 0q�5$  9   $��Center=�6G   ��5  ;   �� $ ParentControl��$+ %DataContext.Online.SortableProperties�Q 5!  9   �� *
Property+5$  G   �� 5&  2   $��1��6;   $O 
Sort Order��5)  9   P . $
��10 0q�5+  9   $��Center=�6G   ��5&  K   �� $ ParentControl��$&  DataContext.Online.SortOperators�Q 5(  9   �� *SortOperator+��5,  6   �� 5-  :   .R�5.  >   �� $	R 1 0q�6H   ��6V   �� # [ 5/  ;   51  3   �� 53  2   $��2��69   $	��5 0q�55  N   $	��2 0q�56  1   $��Bottom=�6?   $��18��6Z   ��54  1   �� $ ParentControl��$# DataContext.RemoveSortCommand���55  1   �� .��57  6     $ Close=�6N   58  3   59  /   5:  +   5<  #   5=     �� 5@     #��[ 6%   $��2��5A  !   $��Right=�5B     $��Center=�69   $��	0 20 0 10q�6T   ��5A  .   �� *AddSortCommand+.��5C  "     $
 Plus=�6:   5D     5F     5G     5H     5I     �� 5L     ���#383838#���  6+   $��0 1 0 0q�6T   $��2��5M      5N     �� 5O     $K E/NyscIdentify.Common.Infrastructure;component/Resources/Material.xaml?�6)   %    ._�5Q     ]� ��.  $
��10 5q�62   .+�5R     �� #��[
 6   $
��Left=�6O   $	��3 0q�5S     $��Center=�6-   $��26��6H   $��26��6S   $	��5 0q�5T     ��6,   �� *ForceTableUpdateCommand+.��5U     G  ANyscIdentify.Common.Infrastructure.Resources.Controls.SpinControl  S   IsBusyS 63   �� *Online.IsBusy+$ PropertyChanged=�.��5V       $
 Sync=�66   5W     5X     �� 5Z     T �� Orientation$T 
Horizontal=�6!   $��Right=�6:   .+�5[     �� $��Right=�6%   $T 
Horizontal=�6<   $
��20 0q�5\  !   $��Center=�6/   .+�5]     �� $��Page Size: ��6(   $��Center=�6;   �� 5^     $��30��6&   $��50��64   $��Center=�6B   $��0q�6]   U �� TextAlignment$U Center=�5_  "   $��5 0 10 4q�69   $��Bottom=�5`  "   ��5_  K   �� *Online.PageSize+$ PropertyChanged=�$
 1000��5`  "   5a     �� 5c     $T 
Horizontal=�6%   $��Center=�6>   .+�5d     �� -   5        $��PART_PreviousButton��5e  D   $��0q�5d  %     TransparentButton#���  69   $
��Auto��5e     $
��Auto��6*   $��3q�68   $
��10 4q�5f     ��6+   �� *PreviousPageCommand+{�5g  %   �� *Online.CanPrevious+$* OneWay=�.��5h  "   D  >NyscIdentify.Common.Infrastructure.Resources.Controls.SvgImage    
ClearImage#���  68     ChevronLeftIconV   	SvgSource#V �  5i  ,   5j     �� 5l     $��Center=�6(   �� 5m  "   �6&   �� *Online.PageNumber+$* OneWay=� �� 5n  "   $	� / ��6&    �� 5o  "   �6&   �� *Online.PageCount+$* OneWay=�5p     �� 5r     $��0q�6%   #���  69   $
��Auto��5s     $
��Auto��6*   $��3q�68   $
��10 4q�6D   ��5t  %   �� *NextPageCommand+{�5u  %   �� *Online.CanNext+$* OneWay=�.��5v  "     #���  68     ChevronRightIcon#V �  5w  ,   5x     5y     5�     5�     5�     5�     5�     5�     