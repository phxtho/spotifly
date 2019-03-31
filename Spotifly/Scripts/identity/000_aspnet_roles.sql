Use spotifly;

CREATE TABLE `AspNetRoles` (
    `Id` varchar(767) NOT NULL,
    `Name` varchar(256) NULL,
    `NormalizedName` varchar(256) NULL,
    `ConcurrencyStamp` text NULL,
    PRIMARY KEY (`Id`)
)ENGINE=InnoDB DEFAULT CHARSET=latin1;
