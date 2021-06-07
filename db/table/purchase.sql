CREATE TABLE `purchase` (
	`Id` INT(10) NOT NULL AUTO_INCREMENT,
	`OrderNo` VARCHAR(50) NOT NULL DEFAULT '' COLLATE 'utf8mb4_unicode_ci' COMMENT '訂單編號',
	`PharmacyId` INT(10) NOT NULL,
	`UserId` INT(10) NOT NULL,
	`TotalQuantity` INT(10) NOT NULL COMMENT '購買商品總數',
	`TotalAmount` DECIMAL(20,4) NOT NULL DEFAULT '0.0000' COMMENT '訂單總額',
	`Status` INT(10) NOT NULL COMMENT '訂單處理狀態',
	`CreateUser` VARCHAR(50) NOT NULL DEFAULT '' COLLATE 'utf8mb4_unicode_ci',
	`CreateTime` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
	`UpdateUser` VARCHAR(50) NOT NULL DEFAULT '' COLLATE 'utf8mb4_unicode_ci',
	`UpdateTime` DATETIME NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	PRIMARY KEY (`Id`) USING BTREE
)
COMMENT='客戶訂單資料'
COLLATE='utf8mb4_unicode_ci'
ENGINE=InnoDB
;