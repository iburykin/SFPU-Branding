<%@Page language="C#" Inherits="Microsoft.SharePoint.Publishing.PublishingLayoutPage, Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%>
<%@Register TagPrefix="PageFieldFieldValue" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%>
<%@Register TagPrefix="Publishing" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%>
<%@Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%>
<asp:Content runat="server" ContentPlaceHolderID="PlaceHolderPageTitle">
            <SharePoint:ProjectProperty Property="Title" runat="server">
            </SharePoint:ProjectProperty>
            
            
            <PageFieldFieldValue:FieldValue FieldName="fa564e0f-0c70-4ab9-b863-0177e6ddd247" runat="server">
            </PageFieldFieldValue:FieldValue>
            
        </asp:Content><asp:Content runat="server" ContentPlaceHolderID="PlaceHolderMain">
            <div class="row">
                <div class="col-sm-12">
                    <!-- First Row -->
                    <div class="row">
                        <div class="col-sm-12">
                            
                            
                            <div>
                                <WebPartPages:WebPartZone runat="server" AllowPersonalization="false" ID="bootstrapRow1" FrameType="None" Orientation="Vertical">
                                    <ZoneTemplate>
                                        
                                    </ZoneTemplate>
                                </WebPartPages:WebPartZone>
                            </div>
                            
                        </div>
                    </div>
                    <!-- Second Row -->
                    <div class="row">
                        <!-- Four Content Areas-->
                        <div class="col-sm-3 col-xs-6">
                            
                            
                            <div>
                                <WebPartPages:WebPartZone runat="server" AllowPersonalization="false" ID="bootstrapRow2Column1" FrameType="None" Orientation="Vertical">
                                    <ZoneTemplate>
                                        
                                    </ZoneTemplate>
                                </WebPartPages:WebPartZone>
                            </div>
                            
                        </div>
                        <div class="col-sm-3 col-xs-6">
                            
                            
                            <div>
                                <WebPartPages:WebPartZone runat="server" AllowPersonalization="false" ID="bootstrapRow2Column2" FrameType="None" Orientation="Vertical">
                                    <ZoneTemplate>
                                        
                                    </ZoneTemplate>
                                </WebPartPages:WebPartZone>
                            </div>
                            
                        </div>
                        <div class="col-sm-3 col-xs-6">
                            
                            
                            <div>
                                <WebPartPages:WebPartZone runat="server" AllowPersonalization="false" ID="bootstrapRow2Column3" FrameType="None" Orientation="Vertical">
                                    <ZoneTemplate>
                                        
                                    </ZoneTemplate>
                                </WebPartPages:WebPartZone>
                            </div>
                            
                        </div>
                        <div class="col-sm-3 col-xs-6">
                            
                            
                            <div>
                                <WebPartPages:WebPartZone runat="server" AllowPersonalization="false" ID="bootstrapRow2Column4" FrameType="None" Orientation="Vertical">
                                    <ZoneTemplate>
                                        
                                    </ZoneTemplate>
                                </WebPartPages:WebPartZone>
                            </div>
                            
                        </div>
                        <!-- End Thumbnails -->
                    </div>
                    <!-- Third Row -->
                    <div class="row">
                        <div class="col-sm-8">
                            <div class="panel radius">
                                <div class="row">
                                    <div class="col-sm-6">
                                        
                                        
                                        <div>
                                            <WebPartPages:WebPartZone runat="server" AllowPersonalization="false" ID="bootstrapRow3Column1Sub1" FrameType="None" Orientation="Vertical">
                                                <ZoneTemplate>
                                                    
                                                </ZoneTemplate>
                                            </WebPartPages:WebPartZone>
                                        </div>
                                        
                                    </div>
                                    <div class="col-sm-6">
                                        
                                        
                                        <div>
                                            <WebPartPages:WebPartZone runat="server" AllowPersonalization="false" ID="bootstrapRow3Column1Sub2" FrameType="None" Orientation="Vertical">
                                                <ZoneTemplate>
                                                    
                                                </ZoneTemplate>
                                            </WebPartPages:WebPartZone>
                                        </div>
                                        
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            
                            
                            <div>
                                <WebPartPages:WebPartZone runat="server" AllowPersonalization="false" ID="bootstrapRow3Column2" FrameType="None" Orientation="Vertical">
                                    <ZoneTemplate>
                                        
                                    </ZoneTemplate>
                                </WebPartPages:WebPartZone>
                            </div>
                            
                        </div>
                    </div>
                </div>
            </div>
        </asp:Content><asp:Content runat="server" ContentPlaceHolderID="PlaceHolderAdditionalPageHead">
            
            
            
            <Publishing:EditModePanel runat="server" id="editmodestyles">
                <SharePoint:CssRegistration name="&lt;% $SPUrl:~sitecollection/Style Library/~language/Themable/Core Styles/editmode15.css %&gt;" After="&lt;% $SPUrl:~sitecollection/Style Library/~language/Themable/Core Styles/pagelayouts15.css %&gt;" runat="server">
                </SharePoint:CssRegistration>
            </Publishing:EditModePanel>
            
        </asp:Content><asp:Content runat="server" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea">
            
            
            <PageFieldFieldValue:FieldValue FieldName="fa564e0f-0c70-4ab9-b863-0177e6ddd247" runat="server">
            </PageFieldFieldValue:FieldValue>
            
        </asp:Content>