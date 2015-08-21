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
	
	<div class="announcements">
	    <xsl:for-each select="$Rows">
			<div class="announcement-item">				
			    <div class="announcement-title">
			    	<span class="date"><xsl:value-of select="@Expires"/></span>:
			    	<xsl:value-of select="@Title"/>
				</div>
			    <div class="announcement-desc">

				    <xsl:variable name="Announcement" select="@Body" />
				    
				    
				    <xsl:call-template name="remove-html">
	    				<xsl:with-param name="text" select="$Announcement"/>
					</xsl:call-template>
					
					 <xsl:choose>				 
				        <xsl:when test="contains($Announcement, '&lt;')">			        
				            <xsl:variable name="AnnouncementPreview" select="substring-before($Announcement, '&lt;')" />
				            <a>
					    		<xsl:attribute name="onclick">
									EditLink2(this,41);return false;
								</xsl:attribute>
					    		<xsl:attribute name="href">
                    ../Lists/Announcements/DispForm.aspx?ID=<xsl:value-of select="@ID"/></xsl:attribute>
								<xsl:value-of select="$AnnouncementPreview" />
							</a>
				        </xsl:when>
				        <xsl:otherwise>
				        	<a>
					    		<xsl:attribute name="onclick">
									EditLink2(this,41);return false;
								</xsl:attribute>
					    		<xsl:attribute name="href">
                    ../Lists/Announcements/DispForm.aspx?ID=<xsl:value-of select="@ID"/></xsl:attribute>
								<xsl:value-of select="$Announcement" />
							</a>				            
				        </xsl:otherwise>
				    </xsl:choose>			  													    
			    </div>
			</div>
			<hr />
		</xsl:for-each>
	    
    </div>
    <div class="more"><a href="../Lists/Announcements">More Events</a></div>
    
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
	<style>
	.announcements {
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
	}
	.announcement-title {
		color: #000000;
		font-weight: bold;
		float: left;
	}
	.more {
		float: right;
		margin-top: 1em;
		margin-bottom: 1em;
		margin-right: 2em;
		display: block;
		background: #27a9e1;
	    background: -o-linear-gradient(top, #27a9e1,  #0077be);
	    background: -ms-linear-gradient(top, #27a9e1,  #0077be);
	    background: -webkit-linear-gradient(top, #27a9e1,  #0077be);
	    background: -moz-linear-gradient(top, #27a9e1,  #0077be);
	    background: linear-gradient(to bottom, #27a9e1,  #0077be);
		color: #FFFFFF;
		font-size: 11pt;
		font-weight: bold;
		-webkit-box-shadow: 0 1px 1px rgba(0, 0, 0, .2);
		-moz-box-shadow: 0 1px 1px rgba(0, 0, 0, .2);	
		box-shadow: 0 1px 1px rgba(0, 0, 0, .2);
		padding: 0.5em 1em;
		border: 1px solid #27a9e1;
		border-radius: 5px;
		-webkit-border-radius: 5px;
		-moz-border-radius: 5px;
	}
	.more a {
		color: #FFFFFF;
		text-decoration: none;
	}
	
  	</style>
  	<script>
  	( function( $ ) {
      $( document ).ready(function() {
		  $("div.announcement").parent().css("background", "#ddd");
		  
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
