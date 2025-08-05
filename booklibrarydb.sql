-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: localhost    Database: booklibrarydb
-- ------------------------------------------------------
-- Server version	8.0.41

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `announcements`
--

DROP TABLE IF EXISTS `announcements`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `announcements` (
  `AnnouncementID` int NOT NULL AUTO_INCREMENT,
  `Message` text NOT NULL,
  `StartDate` timestamp NOT NULL,
  `EndDate` timestamp NOT NULL,
  `CreatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `Title` varchar(255) NOT NULL DEFAULT '',
  `DisplayLocation` varchar(64) NOT NULL DEFAULT '',
  PRIMARY KEY (`AnnouncementID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `announcements`
--

LOCK TABLES `announcements` WRITE;
/*!40000 ALTER TABLE `announcements` DISABLE KEYS */;
INSERT INTO `announcements` VALUES (1,'helooooooo','2025-05-10 21:06:00','2025-05-11 21:06:00','2025-05-10 21:06:11','Announcement','Homepage');
/*!40000 ALTER TABLE `announcements` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `authors`
--

DROP TABLE IF EXISTS `authors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `authors` (
  `AuthorID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  PRIMARY KEY (`AuthorID`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `authors`
--

LOCK TABLES `authors` WRITE;
/*!40000 ALTER TABLE `authors` DISABLE KEYS */;
INSERT INTO `authors` VALUES (1,'Bishal'),(2,'Erin Morgenstern'),(3,'R.L Stine'),(4,'Donna Trett'),(5,'Kristin Hannah'),(6,'Yann Martel'),(7,'J.K Rowling');
/*!40000 ALTER TABLE `authors` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `bookauthor`
--

DROP TABLE IF EXISTS `bookauthor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bookauthor` (
  `BookID` int NOT NULL,
  `AuthorID` int NOT NULL,
  PRIMARY KEY (`BookID`,`AuthorID`),
  KEY `AuthorID` (`AuthorID`),
  CONSTRAINT `bookauthor_ibfk_1` FOREIGN KEY (`BookID`) REFERENCES `books` (`BookID`) ON DELETE CASCADE,
  CONSTRAINT `bookauthor_ibfk_2` FOREIGN KEY (`AuthorID`) REFERENCES `authors` (`AuthorID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bookauthor`
--

LOCK TABLES `bookauthor` WRITE;
/*!40000 ALTER TABLE `bookauthor` DISABLE KEYS */;
INSERT INTO `bookauthor` VALUES (21,2),(22,3),(23,4),(24,5),(25,6),(26,7);
/*!40000 ALTER TABLE `bookauthor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `bookgenre`
--

DROP TABLE IF EXISTS `bookgenre`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bookgenre` (
  `BookID` int NOT NULL,
  `GenreID` int NOT NULL,
  PRIMARY KEY (`BookID`,`GenreID`),
  KEY `GenreID` (`GenreID`),
  CONSTRAINT `bookgenre_ibfk_1` FOREIGN KEY (`BookID`) REFERENCES `books` (`BookID`) ON DELETE CASCADE,
  CONSTRAINT `bookgenre_ibfk_2` FOREIGN KEY (`GenreID`) REFERENCES `genres` (`GenreID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bookgenre`
--

LOCK TABLES `bookgenre` WRITE;
/*!40000 ALTER TABLE `bookgenre` DISABLE KEYS */;
INSERT INTO `bookgenre` VALUES (21,3),(21,4),(22,5),(22,6),(23,7),(24,7),(25,8),(25,9),(26,10),(26,11),(26,12);
/*!40000 ALTER TABLE `bookgenre` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `bookmarks`
--

DROP TABLE IF EXISTS `bookmarks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bookmarks` (
  `UserID` int NOT NULL,
  `BookID` int NOT NULL,
  `CreatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`UserID`,`BookID`),
  KEY `BookID` (`BookID`),
  CONSTRAINT `bookmarks_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`) ON DELETE CASCADE,
  CONSTRAINT `bookmarks_ibfk_2` FOREIGN KEY (`BookID`) REFERENCES `books` (`BookID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bookmarks`
--

LOCK TABLES `bookmarks` WRITE;
/*!40000 ALTER TABLE `bookmarks` DISABLE KEYS */;
/*!40000 ALTER TABLE `bookmarks` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `bookpublisher`
--

DROP TABLE IF EXISTS `bookpublisher`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bookpublisher` (
  `BookID` int NOT NULL,
  `PublisherID` int NOT NULL,
  PRIMARY KEY (`BookID`,`PublisherID`),
  KEY `PublisherID` (`PublisherID`),
  CONSTRAINT `bookpublisher_ibfk_1` FOREIGN KEY (`BookID`) REFERENCES `books` (`BookID`) ON DELETE CASCADE,
  CONSTRAINT `bookpublisher_ibfk_2` FOREIGN KEY (`PublisherID`) REFERENCES `publishers` (`PublisherID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bookpublisher`
--

LOCK TABLES `bookpublisher` WRITE;
/*!40000 ALTER TABLE `bookpublisher` DISABLE KEYS */;
INSERT INTO `bookpublisher` VALUES (21,1),(22,1),(23,1),(24,1),(25,1),(26,1);
/*!40000 ALTER TABLE `bookpublisher` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `books`
--

DROP TABLE IF EXISTS `books`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `books` (
  `BookID` int NOT NULL AUTO_INCREMENT,
  `Title` varchar(255) NOT NULL,
  `ISBN` varchar(13) NOT NULL,
  `Description` text,
  `Language` varchar(50) NOT NULL,
  `PublicationDate` date NOT NULL,
  `Format` varchar(50) NOT NULL,
  `Price` decimal(10,2) NOT NULL,
  `StockQuantity` int NOT NULL,
  `IsPhysical` tinyint(1) NOT NULL,
  `ImageUrl` varchar(255) DEFAULT NULL,
  `CreatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdatedAt` timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `InLibraryAccess` bit(1) NOT NULL DEFAULT b'0',
  `IsPublished` bit(1) NOT NULL DEFAULT b'0',
  `PublishedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`BookID`),
  UNIQUE KEY `ISBN` (`ISBN`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `books`
--

LOCK TABLES `books` WRITE;
/*!40000 ALTER TABLE `books` DISABLE KEYS */;
INSERT INTO `books` VALUES (21,'The Night Circus ','9780451524949','Two illusionists, bound by a magical competition, must navigate a mysterious circus that only appears at night, where their fate is intertwined and only one can survive.','English','2025-05-13','Paperback',400.00,12,1,'/uploads/covers/a7875dca-d7b5-49e9-a35c-45e59de85f34.jpeg',NULL,'2025-05-13 04:27:37',_binary '\0',_binary '','2025-05-13 00:00:00'),(22,'Goosebumps','9780451524935','When Amanda and her brother Josh move to the creepy old house in the town of Dark Falls, they notice that something is terribly wrong. Their new neighbors are unusually pale and act strangely, and the entire town seems trapped in an eerie, shadowy state. As they uncover the town\'s dark secret, they realize that Dark Falls is not just a ghost town — it’s a town full of ghosts. Now, they must escape before they too become permanent residents of the Dead House.','English','2025-05-15','Hardcover',400.00,4,1,'/uploads/covers/2d23a17a-5fd7-4151-af09-cd0eca30a538.jpg',NULL,NULL,_binary '\0',_binary '','2025-05-15 00:00:00'),(23,'The Goldfinch ','9780451524976','After surviving a terrorist bombing at a museum that kills his mother, Theo Decker steals a valuable painting, \"The Goldfinch,\" leading to a life of art, crime, and loss.','English','2025-05-07','Hardcover',600.00,14,1,'/uploads/covers/2b033b7f-16e3-46f0-a1fe-955012b963f3.webp',NULL,NULL,_binary '\0',_binary '','2025-05-07 00:00:00'),(24,'The Nightingale','9780141439518','Two sisters in Nazi-occupied France during World War II take different paths — one joins the Resistance, and the other shelters an enemy pilot, both risking their lives for freedom.','English','2025-05-13','Hardcover',640.00,12,1,'/uploads/covers/5f54d4d2-094d-44b2-a4ad-f3f171be9b59.jpeg',NULL,NULL,_binary '\0',_binary '','2025-05-13 00:00:00'),(25,'Life of Pi','9780451524971','Pi Patel, a young boy stranded on a lifeboat in the Pacific Ocean, must survive for 227 days alongside a Bengal tiger named Richard Parker, exploring themes of faith and survival.','English','2025-05-13','Hardcover',250.00,13,1,'/uploads/covers/f52305c1-3079-451c-99f2-9c8ec835c4ab.jpeg',NULL,NULL,_binary '\0',_binary '','2025-05-13 00:00:00'),(26,'Harry Potter','9780451524953','Harry Potter, an ordinary eleven-year-old boy, discovers that he is actually a wizard and has been accepted to Hogwarts School of Witchcraft and Wizardry. As he begins his magical education, he learns about his parents\' mysterious death, the powerful Dark Lord Voldemort, and the hidden power within him. Alongside his new friends Ron and Hermione, Harry embarks on a thrilling quest to uncover the secrets of the Philosopher\'s Stone, a magical object capable of granting immortality.','English','2025-05-13','Paperback',450.00,43,1,'/uploads/covers/56473053-fc79-4cfc-b42a-c2d4e5563bd2.jpg',NULL,NULL,_binary '\0',_binary '','2025-05-13 07:38:18');
/*!40000 ALTER TABLE `books` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cartitems`
--

DROP TABLE IF EXISTS `cartitems`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cartitems` (
  `UserID` int NOT NULL,
  `BookID` int NOT NULL,
  `Quantity` int NOT NULL,
  `AddedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`UserID`,`BookID`),
  KEY `BookID` (`BookID`),
  CONSTRAINT `cartitems_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`) ON DELETE CASCADE,
  CONSTRAINT `cartitems_ibfk_2` FOREIGN KEY (`BookID`) REFERENCES `books` (`BookID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cartitems`
--

LOCK TABLES `cartitems` WRITE;
/*!40000 ALTER TABLE `cartitems` DISABLE KEYS */;
/*!40000 ALTER TABLE `cartitems` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `discounts`
--

DROP TABLE IF EXISTS `discounts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `discounts` (
  `DiscountID` int NOT NULL AUTO_INCREMENT,
  `BookID` int DEFAULT NULL,
  `StartDate` timestamp NOT NULL,
  `EndDate` timestamp NOT NULL,
  `IsOnSale` tinyint(1) DEFAULT '0',
  `DiscountType` int NOT NULL DEFAULT '0',
  `DiscountValue` decimal(10,2) NOT NULL DEFAULT '0.00',
  `StackingRule` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`DiscountID`),
  KEY `BookID` (`BookID`),
  CONSTRAINT `discounts_ibfk_1` FOREIGN KEY (`BookID`) REFERENCES `books` (`BookID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `discounts`
--

LOCK TABLES `discounts` WRITE;
/*!40000 ALTER TABLE `discounts` DISABLE KEYS */;
/*!40000 ALTER TABLE `discounts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `genres`
--

DROP TABLE IF EXISTS `genres`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `genres` (
  `GenreID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`GenreID`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `genres`
--

LOCK TABLES `genres` WRITE;
/*!40000 ALTER TABLE `genres` DISABLE KEYS */;
INSERT INTO `genres` VALUES (1,'Fictional'),(2,'Comedy'),(3,'Romance'),(4,'Historical Fiction'),(5,'Horror'),(6,'Childrens Fiction'),(7,'Psychological Drama'),(8,'Adventure Fiction'),(9,'Philosophical Fiction'),(10,'Fantasy'),(11,'Adventure'),(12,'Coming-of-Age');
/*!40000 ALTER TABLE `genres` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orderitems`
--

DROP TABLE IF EXISTS `orderitems`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `orderitems` (
  `OrderItemID` int NOT NULL AUTO_INCREMENT,
  `OrderID` int DEFAULT NULL,
  `BookID` int DEFAULT NULL,
  `Quantity` int NOT NULL,
  `UnitPrice` decimal(10,2) NOT NULL,
  PRIMARY KEY (`OrderItemID`),
  KEY `OrderID` (`OrderID`),
  KEY `BookID` (`BookID`),
  CONSTRAINT `orderitems_ibfk_1` FOREIGN KEY (`OrderID`) REFERENCES `orders` (`OrderID`) ON DELETE CASCADE,
  CONSTRAINT `orderitems_ibfk_2` FOREIGN KEY (`BookID`) REFERENCES `books` (`BookID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orderitems`
--

LOCK TABLES `orderitems` WRITE;
/*!40000 ALTER TABLE `orderitems` DISABLE KEYS */;
/*!40000 ALTER TABLE `orderitems` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `orders` (
  `OrderID` int NOT NULL AUTO_INCREMENT,
  `UserID` int DEFAULT NULL,
  `OrderDate` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `Status` enum('Pending','Cancelled','Fulfilled') NOT NULL,
  `ClaimCode` varchar(50) NOT NULL,
  `EmailSent` tinyint(1) DEFAULT '0',
  `TotalAmount` decimal(10,2) NOT NULL,
  `HasFivePlusDiscount` tinyint(1) DEFAULT '0',
  `HasLoyaltyDiscount` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`OrderID`),
  KEY `UserID` (`UserID`),
  CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
INSERT INTO `orders` VALUES (1,11,'2025-05-11 00:59:28','Fulfilled','6FF4933E',0,10.99,0,0),(2,11,'2025-05-11 01:14:27','Fulfilled','10BD4312',0,10.99,0,0),(3,11,'2025-05-11 01:23:57','Fulfilled','25F45602',0,1140.00,1,0),(4,11,'2025-05-11 01:24:33','Fulfilled','22A9142A',0,1088.66,1,0),(5,11,'2025-05-11 01:31:46','Fulfilled','923CB38C',0,740.00,1,0),(6,14,'2025-05-12 09:09:38','Cancelled','807B884E',0,10.99,0,0),(7,6,'2025-05-12 09:53:42','Cancelled','8F2ACE96',0,700.00,0,0),(8,14,'2025-05-12 10:28:56','Cancelled','5626B058',0,0.00,0,0),(9,14,'2025-05-12 12:10:33','Fulfilled','3B68EC90',0,1200.00,0,0),(10,14,'2025-05-12 12:16:46','Pending','7E8D5E7E',0,400.00,0,0);
/*!40000 ALTER TABLE `orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `publishers`
--

DROP TABLE IF EXISTS `publishers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `publishers` (
  `PublisherID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  PRIMARY KEY (`PublisherID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `publishers`
--

LOCK TABLES `publishers` WRITE;
/*!40000 ALTER TABLE `publishers` DISABLE KEYS */;
INSERT INTO `publishers` VALUES (1,'BB publication');
/*!40000 ALTER TABLE `publishers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reviews`
--

DROP TABLE IF EXISTS `reviews`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reviews` (
  `ReviewID` int NOT NULL AUTO_INCREMENT,
  `UserID` int DEFAULT NULL,
  `BookID` int DEFAULT NULL,
  `Rating` int DEFAULT NULL,
  `Comment` text,
  `CreatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`ReviewID`),
  KEY `UserID` (`UserID`),
  KEY `BookID` (`BookID`),
  CONSTRAINT `reviews_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`) ON DELETE CASCADE,
  CONSTRAINT `reviews_ibfk_2` FOREIGN KEY (`BookID`) REFERENCES `books` (`BookID`) ON DELETE CASCADE,
  CONSTRAINT `reviews_chk_1` CHECK (((`Rating` >= 1) and (`Rating` <= 5)))
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reviews`
--

LOCK TABLES `reviews` WRITE;
/*!40000 ALTER TABLE `reviews` DISABLE KEYS */;
/*!40000 ALTER TABLE `reviews` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `staffclaimrecords`
--

DROP TABLE IF EXISTS `staffclaimrecords`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `staffclaimrecords` (
  `ClaimID` int NOT NULL AUTO_INCREMENT,
  `StaffID` int DEFAULT NULL,
  `OrderID` int DEFAULT NULL,
  `ClaimDate` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `ClaimTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  PRIMARY KEY (`ClaimID`),
  KEY `StaffID` (`StaffID`),
  KEY `OrderID` (`OrderID`),
  CONSTRAINT `staffclaimrecords_ibfk_1` FOREIGN KEY (`StaffID`) REFERENCES `users` (`UserID`) ON DELETE CASCADE,
  CONSTRAINT `staffclaimrecords_ibfk_2` FOREIGN KEY (`OrderID`) REFERENCES `orders` (`OrderID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `staffclaimrecords`
--

LOCK TABLES `staffclaimrecords` WRITE;
/*!40000 ALTER TABLE `staffclaimrecords` DISABLE KEYS */;
INSERT INTO `staffclaimrecords` VALUES (1,12,4,'2025-05-11 18:40:01','2025-05-11 18:40:01.359477'),(2,12,5,'2025-05-11 18:49:23','2025-05-11 18:49:23.506228'),(3,15,1,'2025-05-11 19:05:40','2025-05-11 19:05:40.465823'),(4,15,2,'2025-05-11 19:16:53','2025-05-11 19:16:53.167807'),(5,12,3,'2025-05-11 19:18:02','2025-05-11 19:18:02.599047'),(6,15,9,'2025-05-12 18:43:43','2025-05-12 18:43:43.667107');
/*!40000 ALTER TABLE `staffclaimrecords` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `staffnotifications`
--

DROP TABLE IF EXISTS `staffnotifications`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `staffnotifications` (
  `NotificationID` int NOT NULL AUTO_INCREMENT,
  `OrderID` int NOT NULL,
  `ClaimCode` varchar(255) NOT NULL,
  `FulfillmentTime` datetime NOT NULL,
  `Type` varchar(50) NOT NULL DEFAULT 'OrderFulfilled',
  `CreatedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`NotificationID`),
  KEY `OrderID` (`OrderID`),
  CONSTRAINT `staffnotifications_ibfk_1` FOREIGN KEY (`OrderID`) REFERENCES `orders` (`OrderID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `staffnotifications`
--

LOCK TABLES `staffnotifications` WRITE;
/*!40000 ALTER TABLE `staffnotifications` DISABLE KEYS */;
INSERT INTO `staffnotifications` VALUES (1,2,'10BD4312','2025-05-11 19:16:53','OrderFulfilled','2025-05-11 19:16:54'),(2,3,'25F45602','2025-05-11 19:18:03','OrderFulfilled','2025-05-11 19:18:03'),(3,9,'3B68EC90','2025-05-12 18:43:44','OrderFulfilled','2025-05-12 18:43:44');
/*!40000 ALTER TABLE `staffnotifications` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `UserID` int NOT NULL AUTO_INCREMENT,
  `FullName` varchar(100) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `Role` varchar(50) NOT NULL,
  `MembershipID` varchar(50) DEFAULT NULL,
  `SuccessfulOrdersCount` int DEFAULT '0',
  `CreatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `LastLoginDate` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`UserID`),
  UNIQUE KEY `Email` (`Email`),
  UNIQUE KEY `MembershipID` (`MembershipID`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'bb','blbg1046@gmail.com','jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=','Admin','35873939',0,'2025-05-05 01:40:29',NULL),(2,'Shiva','shiva12@gmail.com','jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=','User','12376524',0,'2025-05-05 23:51:02',NULL),(3,'Nishant','nishant1234@gmail.com','jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=','User','40001061',0,'2025-05-09 11:40:42',NULL),(6,'Nishant12','nishant234@gmail.com','jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=','User','62875736',0,'2025-05-09 11:42:15',NULL),(8,'Bishal Bogati','bishal@gmail.com','jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=','Admin','99994812',0,'2025-05-10 13:30:44',NULL),(9,'siri','siri12@gmail.com','jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=','User','94375499',0,'2025-05-10 11:57:30',NULL),(10,'Bogati','bogati@gmail.com','jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=','User','93250291',0,'2025-05-10 11:59:11',NULL),(11,'Yubraj','bhandaribimala967@gmail.com','jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=','User','83124962',0,'2025-05-11 00:49:53',NULL),(12,'Bibek Poudel','movielover1545@gmail.com','NHfl8L68utq0WCl9OO40KiGdQx8uaEiIZlj0TISHvyg=','Staff','22352248',0,'2025-05-11 03:43:26',NULL),(14,'Sudip Bhurtel','siddhant.bhurtel@gmail.com','NHfl8L68utq0WCl9OO40KiGdQx8uaEiIZlj0TISHvyg=','User','80507897',0,'2025-05-11 12:09:40',NULL),(15,'Kritika Basnet','dhirajsubedi36@gmail.com','NHfl8L68utq0WCl9OO40KiGdQx8uaEiIZlj0TISHvyg=','Staff','42456362',0,'2025-05-11 13:08:54',NULL),(16,'Sangita Bhurtel','bhurtelsangita43@gmail.com','NHfl8L68utq0WCl9OO40KiGdQx8uaEiIZlj0TISHvyg=','User','43157891',0,'2025-05-12 12:52:15',NULL);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-05-13 13:27:05
