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
			    	<span class="date"><xsl:value-of select="@Modified"/></span>:
			    	<xsl:value-of select="@Title"/>
				</div>
			    <div class="announcement-desc">
			    	<xsl:variable name="Announcement" select="@Body" />
				    
				    
				    <xsl:call-template name="remove-html">
	    				<xsl:with-param name="text" select="$Announcement"/>
					</xsl:call-template>

					<xsl:value-of select="$Announcement" />				            		  													    
			    </div>
			</div>
			<hr />
		</xsl:for-each>
	    
    </div>
    <div class="more"><a href="../Lists/Events">More Events</a></div>
    
</xsl:template>


<!-- This will remove the tag -->

<xsl:template name="remove-html">
    <xsl:param name="text"/>
    <xsl:choose>
        <xsl:when test="contains($text, '&lt;')">
            <xsl:value-of select="substring-before($text, '&lt;')"/>
            <xsl:call-template name="remove-html">
                    <xsl:with-param name="text" select="substring-after($text, '&gt;')"/>
            </xsl:call-template>
        </xsl:when>
        <xsl:otherwise>
            <xsl:value-of select="$text"/>
        </xsl:otherwise>
    </xsl:choose>
   
</xsl:template>

<xsl:template name="html.tools">
	  	<script>
  	( function( $ ) {
      $( document ).ready(function() {
	      $("div.announcement-desc").each(function( index ) {
	      	var announcement = $(this).text();
	      	if ( announcement.length > 200 ) {		
	      		$(this).text('');    	
		      	$(this).append('<br /><a href="#">' + announcement.substr(0, 200) + '</a>' + '...');
		      }
		   });
      });
      } )( jQuery );
    </script>
</xsl:template>
</xsl:stylesheet>
