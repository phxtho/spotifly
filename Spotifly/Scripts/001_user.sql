CREATE TABLE IF NOT EXISTS user (
id BIGINT NOT NULL AUTO_INCREMENT,
name VARCHAR(255) NOT NULL,
email VARCHAR(255) NOT NULL,
password VARCHAR(128) NOT NULL,
date_created DATETIME DEFAULT NOW() NOT NULL,
CONSTRAINT PK_USER_ID PRIMARY KEY(id),
CONSTRAINT UQ_USER_EMAIL UNIQUE(email)
);

CREATE TABLE IF NOT EXISTS friend (
user1_id BIGINT NOT NULL,
user2_id BIGINT NOT NULL,
CONSTRAINT PK_FRIEND_USER1_USER2 PRIMARY KEY(user1_id, user2_id),
CONSTRAINT FK_FRIEND_USER1_ID FOREIGN KEY (user1_id) REFERENCES `user`(id),
CONSTRAINT FK_FRIEND_USER2_ID FOREIGN KEY (user2_id) REFERENCES `user`(id),
CONSTRAINT CHK_FRIEND_USER1_USER2 CHECK (user1_id > user2_id)
);