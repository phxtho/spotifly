use spotifly;

CREATE TABLE `AspNetUserClaims` (
	`Id` int NOT NULL AUTO_INCREMENT,
	`UserId` varchar(767) NOT NULL,
	`ClaimType` text NULL,
	`ClaimValue` text NULL,
	PRIMARY KEY (`Id`)
)ENGINE=InnoDB DEFAULT CHARSET=latin1;
