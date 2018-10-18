<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:x1="http://malam.com/customs/EAICommon.xsd" xmlns:x2="http://malam.com/customs/INF_MSG_Generic">
  <xsl:output method="html" encoding="windows-1255" indent="yes"/>

  <xsl:template match="/">
    <html>
      <body>
        <xsl:variable name="tbls_">
          <xsl:apply-templates/>
        </xsl:variable>
        <xsl:variable name="tbls" select="$tbls_"/>
        <xsl:call-template name="tbl">
          <xsl:with-param name="t" select="$tbls"/>
        </xsl:call-template>
      </body>
    </html>
  </xsl:template>
  <xsl:template name="tbl">
    <xsl:param name="t"/>
    <xsl:if test="th!=''">
      <table border="1" bordercolor="black" cellpadding="0" cellspacing="0" width="50%">
        <th colspan="2" style="color:red">
          <xsl:value-of select="th"/>
        </th>
        <xsl:for-each select="tr">
          <tr style="color:blue">
            <xsl:for-each select="td">
              <td width="50%">
                <xsl:value-of select="."/>
              </td>
            </xsl:for-each>
          </tr>
        </xsl:for-each>
      </table>
      <br/>
    </xsl:if>
    <xsl:for-each select="$t/table">
      <xsl:if test="name()='table'">
        <xsl:call-template name="tbl">
          <xsl:with-param name="t" select="."/>
        </xsl:call-template>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match="*">
    <xsl:choose>
      <xsl:when test="count(child::*)=0">
        <tr>
          <td>
            <xsl:value-of select="local-name()"/>
          </td>
          <td>
            <xsl:value-of select="text()"/>
            <xsl:value-of select="'&#160;&#160;'"/>
          </td>
        </tr>
      </xsl:when>
      <xsl:otherwise>
        <table border="1" bordercolor="black" cellpadding="0" cellspacing="0">
          <!--<xsl:if test="count(ancestor::*) &gt; 0">-->
          <th colspan="2">
            <xsl:value-of select="local-name()"/>
          </th>
          <!--</xsl:if>-->
          <xsl:apply-templates select="*"/>
        </table>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>