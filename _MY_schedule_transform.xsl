<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" encoding="UTF-8"/>
	<xsl:template match="/Schedules">
		<html>
			<head>
				<title>Schedule Table</title>
				<style>
					table { border-collapse: collapse; width: 100%; }
					th, td { border: 1px solid black; padding: 8px; text-align: left; }
					th { background-color: #f2f2f2; }
				</style>
			</head>
			<body>
				<h2>Schedule Table</h2>
				<table>
					<tr>
						<th>Day</th>
						<th>Course Title</th>
						<th>Room</th>
						<th>Schedule Time</th>
						<th>Student</th>
						<th>Faculty</th>
						<th>Department</th>
						<th>Student Name</th>
						<th>Group</th>
					</tr>
					<xsl:for-each select="Event">
						<xsl:variable name="day" select="@Day"/>
						<xsl:variable name="title" select="@Title"/>
						<xsl:variable name="room" select="@Room"/>
						<xsl:variable name="scheduleTime" select="@ScheduleTime"/>
						<xsl:variable name="StudentName" select="Student/FullName"/>
						<xsl:variable name="faculty" select="Student/@Faculty"/>
						<xsl:variable name="department" select="Student/@Department"/>
						
					</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
