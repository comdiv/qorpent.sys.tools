<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl" xmlns:cs="http://qorpent/dsl/csharp">
	<xsl:output method="text" indent="yes" />

	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="/" xml:space="preserve">	
  <xsl:apply-templates select="//cls" />
  </xsl:template>
	<xsl:template match="cls">
		@@@@/
		<xsl:value-of select="@code" />
		.cs/@@@@namespace X{ public partial class
		<xsl:value-of select="@code" />
		{}}@@@@@@@@
	</xsl:template>

</xsl:stylesheet>