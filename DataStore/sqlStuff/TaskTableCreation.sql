CREATE TABLE "Task" (
	"Id" INTEGER, 
	"Title" TEXT NOT NULL,
	"Description" TEXT NOT NULL,
	"AssignedToUserId"  INTEGER NOT NULL,
	"SourceUserId"  INTEGER NOT NULL,
	"DateCreated"   TEXT NOT NULL,
	"DateAssigned"  TEXT,
 	"DateCompleted" TEXT,
 	"Notes" TEXT,
	PRIMARY KEY("Id" AUTOINCREMENT));


