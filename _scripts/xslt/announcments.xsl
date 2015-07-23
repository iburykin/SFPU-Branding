<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" version="1.0" exclude-result-prefixes="xsl msxsl ddwrt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:SharePoint="Microsoft.SharePoint.WebControls" xmlns:ddwrt2="urn:frontpage:internal">
<xsl:output method="html" indent="no"/>
<xsl:decimal-format NaN=""/>
<xsl:param name="dvt_apos">'</xsl:param>
<xsl:param name="ManualRefresh"></xsl:param>
<xsl:param name="dvt_firstrow">1</xsl:param>
<xsl:param name="dvt_nextpagedata" />
<xsl:variable name="dvt_1_automode">0</xsl:variable>
<xsl:template match="/" xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:SharePoint="Microsoft.SharePoint.WebControls">

	<xsl:variable name="Rows" select="/dsQueryResponse/Rows/Row"/>
	<xsl:variable name="RowsCount" select="count($Rows)"/>
	
	<xsl:call-template name="html.tools"/>
	<div class="announcements">
	    <xsl:for-each select="$Rows">
			<div class="announcement-item">
			    <div class="announcement-title">
			    	<a>
			    		<xsl:attribute name="onclick">
							EditLink2(this,41);return false;
						</xsl:attribute>
			    		<xsl:attribute name="href">../Lists/Announcements/DispForm.aspx?ID=<xsl:value-of select="@Title"/></xsl:attribute>
						<xsl:value-of select="@Title"/>
					</a>
				</div>
			    <div class="announcement-desc"><span class="date"><xsl:value-of select="@Date"/></span>
			    <p><xsl:value-of select="@Announcement" disable-output-escaping="yes"/></p>
			    </div>
			</div>
		</xsl:for-each>
	    <div class="more"><a href="../Lists/Announcements">More...</a></div>
    </div>
    
</xsl:template>

<xsl:template name="html.tools">
	<style>
	.announcements {
		width: 100%;
		margin: 0;
		padding: 1em 0.5em;
	}
  	</style>
</xsl:template>
</xsl:stylesheet>
