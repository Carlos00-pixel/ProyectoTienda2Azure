CREATE TABLE Artista(
	IdArtista int NOT NULL,
	Nombre nvarchar(50) NULL,
	Apellidos varchar(50) NULL,
	Nick nvarchar(50) NULL,
	Descripcion nvarchar(700) NULL,
	Email nvarchar(100) NULL,
	Password varbinary(max) NULL,
	Salt nvarchar(50) NULL,
	Imagen nvarchar(400) NULL,
 CONSTRAINT PK_Artista PRIMARY KEY CLUSTERED 
(
	IdArtista ASC
))
GO
/****** Object:  Table Info_Arte    Script Date: 19/03/2023 21:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE Info_Arte(
	IdInfoArte int NOT NULL,
	Titulo nvarchar(50) NULL,
	Precio int NULL,
	Descripcion nvarchar(700) NULL,
	Imagen nvarchar(700) NULL,
	IdArtista int NULL,
 CONSTRAINT PK_Info_Arte PRIMARY KEY CLUSTERED 
(
	IdInfoArte ASC
))
GO
/****** Object:  View INFO_PRODUCTOS    Script Date: 19/03/2023 21:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW INFO_PRODUCTOS
AS
	SELECT TOP 100 PERCENT 
	IA.IdInfoArte, IA.Titulo, IA.Precio, IA.Imagen, IA.Descripcion, A.IdArtista, A.Nick
	FROM Info_Arte IA
	INNER JOIN Artista A
	ON IA.IdArtista = A.IdArtista
	order by IA.IdInfoArte desc
GO
/****** Object:  Table Chat    Script Date: 19/03/2023 21:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE Chat(
	IdChat int NOT NULL,
	Mensajes nvarchar(500) NULL,
	IdReceptor int NULL,
	IdSubscriptor int NULL,
	Fecha_Hora date NULL,
	IdArtista int NULL,
	IdCliente int NULL,
 CONSTRAINT PK_Chat PRIMARY KEY CLUSTERED 
(
	IdChat ASC
))
GO
/****** Object:  Table Cliente    Script Date: 19/03/2023 21:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE Cliente(
	IdCliente int NOT NULL,
	Nombre nvarchar(50) NULL,
	Apellidos nvarchar(50) NULL,
	Email nvarchar(100) NULL,
	Password varbinary(max) NULL,
	Salt nvarchar(50) NULL,
	Imagen nvarchar(400) NULL,
 CONSTRAINT PK_Cliente PRIMARY KEY CLUSTERED 
(
	IdCliente ASC
))
GO
/****** Object:  Table Transaccion    Script Date: 19/03/2023 21:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE Transaccion(
	IdTransaccion int NOT NULL,
	Fecha_Transaccion date NULL,
	Precio_Compra int NULL,
	Precio_Venta int NULL,
	IdCliente int NULL,
	IdInfoArte int NULL,
 CONSTRAINT PK_Transaccion PRIMARY KEY CLUSTERED 
(
	IdTransaccion ASC
))
GO
/****** Object:  Table Valoraciones    Script Date: 19/03/2023 21:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE Valoraciones(
	IdValoraciones int NOT NULL,
	Puntuacion int NULL,
	IdCliente int NULL,
	IdArtista int NULL,
 CONSTRAINT PK_Valoraciones PRIMARY KEY CLUSTERED 
(
	IdValoraciones ASC
))
GO
INSERT Artista (IdArtista, Nombre, Apellidos, Nick, Descripcion, Email, Password, Salt, Imagen) VALUES (1, N'Gabriel', N'Picolo', N'_picolo', N'Artista
• 🇧🇷 brazilian comic artist', N'gabrielpicolo@gmail.com', 0x31003200330034003500, NULL, N'https://pbs.twimg.com/profile_images/1148295233727270913/vM-65pZl_bigger.jpg')
INSERT Artista (IdArtista, Nombre, Apellidos, Nick, Descripcion, Email, Password, Salt, Imagen) VALUES (2, N'Laia', N'Lopez', N'itslopez', N'Artista
Barcelona, Spain
• illustration, ch.design
• inquiries: 📩 itslopezillustrations@gmail.com
• Rep’d by Britt Siess Creative Management', N'itslopezillustrations@gmail.com', 0x31003200330034003500, NULL, N'https://i.pinimg.com/originals/c5/46/e3/c546e3ccacab573ebe52d33834501a0d.jpg')
INSERT Artista (IdArtista, Nombre, Apellidos, Nick, Descripcion, Email, Password, Salt, Imagen) VALUES (3, N'Zune', NULL, N'creandoinfinito', N'Artista
¡Soy Zune! Hago ilustración. También me gusta el modelar con plastilina. Me gustan los retos, los colores locos y probar cosas nuevas.                                 ', N'creandoinfinito@gmail.com', 0x904D4C8B9E5ADD899A57D788A4FC33F2AB1EAECE3FD11673703B12E3DE897FE447F122DDC20E5E4D0C9DC43A003BD2FBFF1CCD069F2814C2D7DA1A10E4AC751F, N'Èì=týC
MxMðYZ?´¼«æ¸ÂZYåÀ+&½À1ü=', N'https://ksr-ugc.imgix.net/assets/039/254/628/b9c47e7474b8b810ea97ecd4d7bad9bc_original.png?ixlib=rb-4.0.2&w=80&h=80&fit=crop&v=1678360852&auto=format&frame=1&q=92&s=71d1c160c16638bd334366cd0c1efbe3')
GO
INSERT Cliente (IdCliente, Nombre, Apellidos, Email, Password, Salt, Imagen) VALUES (1, NULL, NULL, NULL, 0xD04CFADAB37F5A5880927C1158D031CE7F62C1D8D261A9F2CFA42E875AFBEB520968EA02991658167A0670694E004AFE435A95464D0E5E1F62AE6416A907CD3F, N'^mØµ£5SHQ»Uñ,Uî
DZ£!y`ë³Æfúí_''ðz#4BµU¼', NULL)
INSERT Cliente (IdCliente, Nombre, Apellidos, Email, Password, Salt, Imagen) VALUES (2, N'Ejemplo', N'Prueba Prueba', N'ejemploprueba@gmail.com', 0x391AAC97930CF1D81DA00F4F1F3FE08E80B23278990B9880F0507003021A6AF50A5B19D7B2AA3E9605DC70BF416061142DDCBE5E8ADE1EEBCE3F2207BDBFFA56, N'rJ*ÎFDøÎÔ}''ÔöWý®®ÐÌ½ZDyI>öAê¸2¿|{­é(w@', N'https://ceslava.s3-accelerate.amazonaws.com/2016/04/mistery-man-gravatar-wordpress-avatar-persona-misteriosa-510x510.png')
GO
INSERT Info_Arte (IdInfoArte, Titulo, Precio, Descripcion, Imagen, IdArtista) VALUES (1, N'Summer 1999', 35, N'Giclee print
16 x 15 inches
﻿Hand-numbered limited edition of 100
Ships in 2 - 3 weeks', N'https://cdn.shopify.com/s/files/1/0230/6428/1166/products/digi-2_540x.jpg?v=1564779773', 1)
INSERT Info_Arte (IdInfoArte, Titulo, Precio, Descripcion, Imagen, IdArtista) VALUES (2, N'Get Jinx', 6, N'Jinx de Arcane', N'https://i.pinimg.com/originals/e7/6f/94/e76f944701bc1026dfe0473e68e72a47.jpg', 2)
INSERT Info_Arte (IdInfoArte, Titulo, Precio, Descripcion, Imagen, IdArtista) VALUES (3, N'Jim Hawkins', 17, N'Jim Hawkins from Treasure Planet', N'https://pbs.twimg.com/media/Fi058moXEAMPnJJ?format=jpg&name=large', 1)
INSERT Info_Arte (IdInfoArte, Titulo, Precio, Descripcion, Imagen, IdArtista) VALUES (4, N'Beast Boy & Pets', 30, N'Giclee print
13 x 11 inches
Open edition', N'https://cdn.shopify.com/s/files/1/0230/6428/1166/products/beastboy-shadow_540x.jpg?v=1557848357', 1)
INSERT Info_Arte (IdInfoArte, Titulo, Precio, Descripcion, Imagen, IdArtista) VALUES (5, N'Major Arcana Cats - Tarot', 20, N'A deck of 22 cat-cards inspired by the major arcana. Una baraja de 22 cartas gatunas inspiradas en los arcanos mayores.', N'https://ksr-ugc.imgix.net/assets/040/180/074/90ab1177e65767c5d6466967728677cf_original.png?ixlib=rb-4.0.2&crop=faces&w=1024&h=576&fit=crop&v=1678360742&auto=format&frame=1&q=92&s=36fc7b41489dfc2480230b24d2fbae1d', 3)
GO
ALTER TABLE Chat  WITH CHECK ADD  CONSTRAINT FK_Chat_Artista FOREIGN KEY(IdArtista)
REFERENCES Artista (IdArtista)
GO
ALTER TABLE Chat CHECK CONSTRAINT FK_Chat_Artista
GO
ALTER TABLE Chat  WITH CHECK ADD  CONSTRAINT FK_Chat_Cliente FOREIGN KEY(IdCliente)
REFERENCES Cliente (IdCliente)
GO
ALTER TABLE Chat CHECK CONSTRAINT FK_Chat_Cliente
GO
ALTER TABLE Info_Arte  WITH CHECK ADD  CONSTRAINT FK_Info_Arte_Artista FOREIGN KEY(IdArtista)
REFERENCES Artista (IdArtista)
GO
ALTER TABLE Info_Arte CHECK CONSTRAINT FK_Info_Arte_Artista
GO
ALTER TABLE Transaccion  WITH CHECK ADD  CONSTRAINT FK_Transaccion_Cliente FOREIGN KEY(IdCliente)
REFERENCES Cliente (IdCliente)
GO
ALTER TABLE Transaccion CHECK CONSTRAINT FK_Transaccion_Cliente
GO
ALTER TABLE Transaccion  WITH CHECK ADD  CONSTRAINT FK_Transaccion_Info_Arte FOREIGN KEY(IdInfoArte)
REFERENCES Info_Arte (IdInfoArte)
GO
ALTER TABLE Transaccion CHECK CONSTRAINT FK_Transaccion_Info_Arte
GO
ALTER TABLE Valoraciones  WITH CHECK ADD  CONSTRAINT FK_Valoraciones_Artista FOREIGN KEY(IdArtista)
REFERENCES Artista (IdArtista)
GO
ALTER TABLE Valoraciones CHECK CONSTRAINT FK_Valoraciones_Artista
GO
ALTER TABLE Valoraciones  WITH CHECK ADD  CONSTRAINT FK_Valoraciones_Cliente FOREIGN KEY(IdCliente)
REFERENCES Cliente (IdCliente)
GO
ALTER TABLE Valoraciones CHECK CONSTRAINT FK_Valoraciones_Cliente
GO