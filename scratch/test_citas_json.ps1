$connectionString = "Server=DESKTOP-ESKNKL3\MSSQLSERVER03;Database=Epsilon;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;"
try {
    $conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
    $conn.Open()

    $cmd = $conn.CreateCommand()
    $cmd.CommandText = @"
SELECT c.IdCita, c.FechaInicio, c.FechaFin, p.NombrePaciente, m.NombreMedico, t.NombreTratamiento, t.Color
FROM Citas c
LEFT JOIN Pacientes p ON c.IdPaciente = p.IdPaciente
LEFT JOIN Medicos m ON c.IdMedico = m.IdMedico
LEFT JOIN CitaTratamientos ct ON c.IdCita = ct.IdCita
LEFT JOIN Tratamientos t ON ct.IdTratamiento = t.IdTratamiento
"@
    $reader = $cmd.ExecuteReader()
    Write-Host "--- Citas con Detalles ---"
    while ($reader.Read()) {
        Write-Host "Cita $($reader['IdCita']) | Inicio: $($reader['FechaInicio']) | Pac: $($reader['NombrePaciente']) | Med: $($reader['NombreMedico']) | Trat: $($reader['NombreTratamiento']) | Color: $($reader['Color'])"
    }
    $reader.Close()

    $conn.Close()
} catch {
    Write-Host "Error: $_"
}
