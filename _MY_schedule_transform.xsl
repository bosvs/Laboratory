<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
		<html>
			<head>
				<style>
					table { border-collapse: collapse; width: 100%; }
					th, td { border: 1px solid black; padding: 8px; text-align: left; }
					th { background-color: #f2f2f2; }
				</style>
			</head>
			<body>
				<table>
					<tr>
						<th>Name</th>
						<th>Faculty</th>
						<th>Department</th>
						<th>Title</th>
						<th>Room</th>
						<th>Day</th>
						<th>Schedule Time</th>
					</tr>
					<xsl:for-each select="/Schedule/Event">
						<tr>
							<td>
								<xsl:value-of select="Student/FullName"/>
							</td>
							<td>
								<xsl:value-of select="Student/@Faculty"/>
							</td>
							<td>
								<xsl:value-of select="Student/@Department"/>
							</td>
							<td>
								<xsl:value-of select="@Title"/>
							</td>
							<td>
								<xsl:value-of select="@Room"/>
							</td>
							<td>
								<xsl:value-of select="@Day"/>
							</td>
							<td>
								<xsl:value-of select="@ScheduleTime"/>
							</td>
						</tr>
					</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
