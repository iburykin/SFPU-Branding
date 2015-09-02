<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" version="1.0" 
                exclude-result-prefixes="xsl msxsl ddwrt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" 
                xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" 
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:SharePoint="Microsoft.SharePoint.WebControls" 
                xmlns:ddwrt2="urn:frontpage:internal">
<xsl:output method="html" indent="no"/>
<xsl:decimal-format NaN=""/>
<xsl:param name="dvt_apos">'</xsl:param>
<xsl:param name="ManualRefresh"></xsl:param>
<xsl:param name="dvt_firstrow">1</xsl:param>
<xsl:param name="dvt_nextpagedata" />
<xsl:variable name="dvt_1_automode">0</xsl:variable>
<xsl:template match="/" xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" 
              xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" 
              xmlns:SharePoint="Microsoft.SharePoint.WebControls">

<xsl:variable name="Rows" select="/dsQueryResponse/Rows/Row"/>
<xsl:variable name="RowsCount" select="count($Rows)"/>
	
<xsl:call-template name="html.tools"/>
	<div class="linksListView">
		<ul>
		    <xsl:for-each select="$Rows">
				<li>
          <a>
            <xsl:attribute name="href">
              <xsl:value-of select="@URL"/>
            </xsl:attribute>
            <xsl:value-of select="@URL.desc"   disable-output-escaping="yes"/>
          </a>
				</li>			
			</xsl:for-each>
	    </ul>
    </div>
    <div class="more"><a href="../Lists/Links">More Links</a></div>
</xsl:template>

<xsl:template name="html.tools">
	<style>
		.linksListView {
			width: 90%;
			margin: 0 auto;
			margin-top: 1em;
			padding: 1em 0.5em;
			background: #ffffff;
			height: 200px;
			overflow-y: scroll;
			white-space: normal;
			word-break: break-word;
			border: solid 1px #c6c6c6;
			line-height: 1.5;
		}
		.linksListView ul li {
			list-style:none;
		}

		.linksListView ul li:before {
			color:#FDBA1C;
			content:"\2022";
			font-size:1.5em;
			padding-right:.25em;
			position:relative;
			top:.1em;
		}
	</style>
</xsl:template>



</xsl:stylesheet>
