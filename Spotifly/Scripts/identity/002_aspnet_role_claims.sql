CREATE TABLE `AspNetRoleClaims` (
`Id` int NOT NULL AUTO_INCREMENT,
`RoleId` varchar(767) NOT NULL,
`ClaimType` text NULL,
`ClaimValue` text NULL,
PRIMARY KEY (`Id`),
CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
)ENGINE=InnoDB DEFAULT CHARSET=latin1;