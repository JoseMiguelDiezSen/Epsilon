$connectionString = "Server=DESKTOP-ESKNKL3\MSSQLSERVER03;Database=Epsilon;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;"
try {
    $conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
    $conn.Open()

    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT * FROM Clinicas"
    $reader = $cmd.ExecuteReader()
    Write-Host "--- Clinicas ---"
    while ($reader.Read()) {
        Write-Host "IdClinica: $($reader['IdClinica']) | Nombre: $($reader['NombreClinica'])"
    }
    $reader.Close()

    $cmd.CommandText = "SELECT name, is_identity FROM sys.columns WHERE object_id = OBJECT_ID('Citas') AND name = 'IdCita'"
    $isIdent = $cmd.ExecuteScalar()
    Write-Host "Citas.IdCita is identity: $isIdent"

    $cmd.CommandText = "SELECT name, is_identity FROM sys.columns WHERE object_id = OBJECT_ID('Agenda') AND name = 'IdAgenda'"
    $isIdentAgenda = $cmd.ExecuteScalar()
    Write-Host "Agenda.IdAgenda is identity: $isIdentAgenda"

    $cmd.CommandText = "SELECT name, is_identity FROM sys.columns WHERE object_id = OBJECT_ID('CitaTratamientos') AND name = 'IdCitaTratamiento'"
    $isIdentCT = $cmd.ExecuteScalar()
    Write-Host "CitaTratamientos.IdCitaTratamiento is identity: $isIdentCT"

    $conn.Close()
} catch {
    Write-Host "Error: $_"
}
