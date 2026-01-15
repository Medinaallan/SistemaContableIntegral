UPDATE Usuarios SET PasswordHash = '$2a$11$DUVylZlu1p4oZiii36qv7.hNb8Kyi4jrkIQGocLkafFJ/YV5wZWtC' WHERE Id = 1;
SELECT Id, NombreUsuario, LEFT(PasswordHash, 30) as Hash FROM Usuarios;
