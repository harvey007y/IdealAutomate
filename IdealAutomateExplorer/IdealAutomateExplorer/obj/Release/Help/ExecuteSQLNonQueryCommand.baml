   M S B A M L   `   `   `  ����   IdealAutomateExplorer:    4Hardcodet.Wpf.Samples.Help.ExecuteSQLNonQueryCommand   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       N� Title$  ExecuteSQLNonQueryCommand��.��-�  -� ViewingMode$ Scroll=�.��/�  /� FontSize$ 20(�.T�J� .*�.*��� $��/Images/help1.png��$	��190��$	��190��$��0,0,30,0q� �� $��30(� �� BaselineAlignment$ Center=�.�ExecuteSQLNonQueryCommandJ� .*�.*��� $��24(�.�	Examples:�� .<��� .9�J� .*�.*��� .�ExecuteSQLNonQueryCommand��  �� CellSpacing$ 5�� �� BorderThickness$ 1q� �� BorderBrush
 �   �.��� .��� .��� .�J� .*�.*��� $!��/Images/AddSQLParameter.PNG��$��5,5,5,5q�$��Center=��� $ 0��$ 1q�
 �   �.��� .��� 
�������$
��Bold"�.���  �� BorderBrush
 �   � �� BorderThickness$ 1q�.�J� 	 �� TextAlignment$	 Center=�.*�Verb�� 
 �   �$ 1q�.�J� $	 Center=�.*�	Entity1�� 
 �   �$ 1q�.�J� $	 Center=�.*�Relationship�� 
 �   �$ 1q�.�J� $	 Center=�.*�	Entity2�� .��� .��� 
 �   �$ 1q�.�J� .*�ExecuteSQLNonQueryCommand�� 
 �   �$ 1q�.�J� .*�sqlCmdUpdate�� 
 �   �$ 1q�.�J� �� 
 �   �$ 1q�.�J� J� .*�.*��� .�Explanation of example -�� TRStatement 1: clears all of the sql parameters for the next sql command to execute.�� TRStatement 2: adds the sqlParmFirstName parameter that is defined in SQL Parms tab.�� ECStatement 3: executes sqlCmdUpdate that is defined in SQL Cmds tab.�� ��Statement 4: displays the ReturnCode, which indicates the number of rows affected, if there are no errors; otherwise, it displays a negative error code.�� TRStatement 5: clears all of the sql parameters for the next sql command to execute.�� MKStatement 6: executes the sqlCmdSelect that is defined in the SQL Cmds tab.�� 31Statement 7: sets the row index for arySqlResult.�� PNStatement 8: sets the column index to -1, which causes all columns to be used.�� trStatement 9: sets all of the children elements under the parent object myColumns to the third row of arySqlResult.�� ��Statement 10: displays the child element name whose parent is myColumns. This displays the column that we updated in Statement 3.�� J� .*�.*��� 
 �� Stretch$

 Fill=�
���   �	 �� X2$ 1��J� .*�.*��� $��24(�.�
Columns:J� .*�.*��� .�ExecuteSQLNonQueryCommand�� - ExecuteSQLNonQueryCommand is used to execute a SQL Query Command that does not return any rows to the script. Entity1 is the name of a SQL Command that has been defined in the SQL Cmds tab. Here are the columns:J� $
��14pt(�.*�.*��� .�.��� .�1. SequenceJ� .*���Sequence is a decimal number with 4 digits after the decimal. It can be modified to allow you to insert rows anywhere in your script. Rows are executed in sequence number order.J� $
��14pt(�.*�.*��� .�.��� .�	2. VerbJ� .*�ExecuteSQLNonQueryCommandJ� $
��14pt(�.*�.*��� .�.��� .�
3. Entity1J� .*�USEntity1 is the name of a SQL command that has been defined in the SQL Commands tab.J� $
��14pt(�.*�.*��� .�.��� .�4. RelationshipJ� .*�Relationship is not used.J� $
��14pt(�.*�.*��� .�.��� .�
5. Entity2J� .*�Entity2 is not used.