<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SVAMITVA._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>SVAMITVA</h1>
        <p class="lead">
            SVAMITVA Scheme 
        </p>

    </div>

    <div class="row">
        <div class="col-lg-6">
            <div class="form-group">
                <span>*</span>
                <asp:RequiredFieldValidator ID="rfvddlPropDistrict" runat="server" ControlToValidate="ddlPropDistrict"
                    ErrorMessage="Please Select Property District" ValidationGroup="ErrorProp" InitialValue="0">*</asp:RequiredFieldValidator>
                <label for="ddlPropDistrict" class="control-label">District</label>
                <div class="">
                    <asp:DropDownList ID="ddlPropDistrict" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlPropDistrict_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>

                </div>
            </div>
        </div>
        <div class="col-lg-6">
            <div class="form-group">
                <span>*</span>
                <asp:RequiredFieldValidator ID="rfvddlPropTehsil" runat="server" ControlToValidate="ddlPropTehsil"
                    ErrorMessage="Please Select Property Tehsil" ValidationGroup="ErrorProp" InitialValue="0">*</asp:RequiredFieldValidator>
                <label for="ddlPropTehsil" class="control-label">Tehsil</label>
                <div class="">
                    <asp:DropDownList ID="ddlPropTehsil" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlPropTehsil_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </div>
            </div>
        </div>
    </div>
    <div class="row">

        <div class="col-lg-6">
            <div class="form-group">
                <span>*</span>
                <asp:RequiredFieldValidator ID="rfvddlPropVillage" runat="server" ControlToValidate="ddlPropVillage"
                    ErrorMessage="Please Select Property Village" ValidationGroup="ErrorProp" InitialValue="0">*</asp:RequiredFieldValidator>
                <label for="ddlPropVillage" class="control-label">Village/Town</label>
                <div class="">
                    <asp:DropDownList ID="ddlPropVillage" runat="server" CssClass="form-control js-example-basic-single" AutoPostBack="true" OnSelectedIndexChanged="ddlPropVillage_SelectedIndexChanged"></asp:DropDownList>

                </div>
            </div>
        </div>
        <div class="col-lg-6">
            <div class="form-group">
                <label for="txtHadbastNo" class="control-label">Hadbast Number</label>
                <asp:TextBox ID="txtHadbastNo" runat="server" CssClass="form-control" placeholder="HadbastNo No" MaxLength="150"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                    ControlToValidate="txtHadbastNo"
                    ValidationExpression="^[^#%&*:<>?/{|}]+$"
                    Display="Static"
                    ErrorMessage="Please Enter Correct Hadbast Number"
                    EnableClientScript="False"
                    runat="server" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-6">
            <div class="form-group">
                <span>*</span>
                <asp:RequiredFieldValidator ID="rfvSurveyUnittype" runat="server" ControlToValidate="ddlSurveyUnittype"
                    ErrorMessage="Please Select Survey Unit type" ValidationGroup="ErrorProp" InitialValue="0">*</asp:RequiredFieldValidator>
                <label for="ddlSurveyUnittype" class="control-label">Survey Unit Type</label>
                <div class="">
                    <asp:DropDownList ID="ddlSurveyUnittype" runat="server" CssClass="form-control js-example-basic-single"></asp:DropDownList>

                </div>
            </div>
        </div>
        <div class="col-lg-6">
            <div class="form-group">
                <span>*</span>
                <asp:RequiredFieldValidator ID="rfvSurveyUnitNo" runat="server" ControlToValidate="txtSurveyUnitNo"
                    ErrorMessage="Please Enter Survey Unit Number" ValidationGroup="ErrorProp">*</asp:RequiredFieldValidator>
                <label for="SurveyUnitNo" class="control-label">Survey Unit Number</label>
                <div class="">
                    <asp:TextBox ID="txtSurveyUnitNo" runat="server" CssClass="form-control" placeholder="SurveyUnitNo" MaxLength="150"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="regexSurveyUnitNo"
                        ControlToValidate="txtSurveyUnitNo"
                        ValidationExpression="^[^#%&*:<>?/{|}]+$"
                        Display="Static"
                        ErrorMessage="Please Enter Correct Survey Unit Number"
                        EnableClientScript="False"
                        runat="server" />

                </div>
            </div>

        </div>

    </div>
</asp:Content>
