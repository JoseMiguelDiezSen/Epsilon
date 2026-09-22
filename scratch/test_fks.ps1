$connectionString = "Server=DESKTOP-ESKNKL3\MSSQLSERVER03;Database=Epsilon;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;"
try {
    $conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
    $conn.Open()

    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT name, definition FROM sys.foreign_keys fk CROSS APPLY (SELECT fk.name, OBJECT_NAME(fk.parent_object_id) AS parent_table, OBJECT_NAME(fk.referenced_object_id) AS referenced_table) t WHERE parent_table = 'Citas' OR parent_table = 'Agenda' OR parent_table = 'CitaTratamientos'"
    $cmd.CommandText = @"
SELECT 
    f.name AS ForeignKeyName,
    OBJECT_NAME(f.parent_object_id) AS TableName,
    COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ColumnName,
    OBJECT_NAME (f.referenced_object_id) AS ReferenceTableName,
    COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS ReferenceColumnName
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc 
    ON f.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) IN ('Citas', 'Agenda', 'CitaTratamientos');
"@
    $reader = $cmd.ExecuteReader()
    Write-Host "--- Foreign Keys ---"
    while ($reader.Read()) {
        Write-Host "$($reader['TableName']).$($reader['ColumnName']) -> $($reader['ReferenceTableName']).$($reader['ReferenceColumnName'])"
    }
    $reader.Close()

    $conn.Close()
} catch {
    Write-Host "Error: $_"
}
