   M S B A M L   `   `   `  ����U  QSocketPrograming_ServerWpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:SocketPrograming_ServerWpfSocketPrograming_ServerWpf  5   	   +    %SocketPrograming_ServerWpf.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   :local(clr-namespace:SocketPrograming_ServerWpf     5   	     � Title$  ServerWindow��5   	   $	��400��6   $	��600��6+   .��5	      � ��5
      �� 5   
    �� ImageSource$- 'Image\Win11-background-3-min-scaled.jpg��6   .+�5      �� $	��150��6   $
��Left=�6   .+�5   
   �� $
��IP :��6   
�������$��18(�63   $��10,0,0,0q�6A   6U   �� 5   
   -   5        $��txtIP��5      $��10,0,0,0q�6   $��	127.0.0.1��61   6D   �� 5   
   $��Port :��6   
�������$��18(�65   $��10,0,0,0q�6C   6W   �� 5   
   -   5        $��txtPort��5      $��10,0,0,0q�6!   $
��8080��63   6A   �� 5   
   -   5        $��btnStop��5      $	��100��5      $��30��6   $��Center=�5   	   $��	10,10,0,0q�6&   $
��Stop��5      
�������$��18(�5   	   ��5   	   Z� 5      �� 5   
   -   5         ��Name$ btnStart��5   #   $	��100��5      $��30��6   $��Start��5   +   
�������$��18(�5      $��	10,10,0,0q�65   ��5      Z� 5   5   �� 5   
   -   5        $��btnExit��5      $	��100��5      $��30��6   $��Center=�5   	   $��	10,10,0,0q�6&   $
��Exit��5      
�������$��18(�5   	   ��5   	   Z� 5      �� 5    
   $��Users:��6   
�������$��18(�65   $��	30,10,0,0q�6C   6Y   �� 5!   
   -   5        $��txtListUser��5!   7   $	��126��6   $	��131��6   $��10q�6+   
���������6J   Z� 6u   5"      �� 5$      $
��Left=�6   $	��380��6,   $��	220,0,0,0q�68   .+�5%   
   �� $��Server��6   
�������$��30(�65   $��	30,10,0,0q�6C   $��Center=�6V   6u   �� 5&   
   -   5        $��txtListMessage��5&   A   $	��300��6   $	��214��6   $��	0,10,40,0q�6W   
�������$��Right=�6}   ��6+   Z� 6�   �� 5(   
   -   5        $��
txtmessage��5(   +   $	��300��6   $��30��6   $��10q�6=   6L   �� 5)   
   -	   5        $��sendMessage��5,   &   $��60��5)      $��30��6   $��Center=�5*      $
��Send��5+      
�������$��18(�5,      ���	#FF02172F6&   5.      5/      50      