CREATE DATABASE `template` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;

USE `template`;

CREATE TABLE `person` (
  `person_id` char(36) NOT NULL,
  `username` varchar(45) DEFAULT NULL,
  `password` varchar(256) DEFAULT NULL,
  PRIMARY KEY (`person_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `todo` (
  `todo_id` char(36) NOT NULL,
  `title` varchar(45) DEFAULT NULL,
  `body` longtext,
  `person_id` char(36) DEFAULT NULL,
  PRIMARY KEY (`todo_id`),
  KEY `fk_todo_person` (`person_id`),
  CONSTRAINT `fk_todo_person` FOREIGN KEY (`person_id`) REFERENCES `person` (`person_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
