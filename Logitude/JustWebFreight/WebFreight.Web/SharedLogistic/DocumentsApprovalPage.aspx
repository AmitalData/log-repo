<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentsApprovalPage.aspx.cs" Inherits="WebFreight.Web.SharedLogistic.DocumentsApprovalPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <meta http-equiv="X-UA-Compatible" content="IE=edge" />

    <title>Documents Approval</title>

</head>
<body style="background: #F2F2F2;" class="LogitudeWindow">

    <div>
        <table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td style="width: 5px;"></td>

                            <td style="vertical-align: central;">
                                <div>
                                    <span style="font-size: 15px; display: inline; color: black;">Please approve to download/view documents. </span>
                                </div>
                            </td>

                            <td style="width: 10px;"></td>
                        </tr>

                        <tr>
                            <td style="width: 5px;"></td>

                            <td style="vertical-align: central;">
                                <div>
                                    <span style="font-size: 12px; display: inline; color: black">(Shipment received confirmation will be sent) </span>
                                </div>
                            </td>

                            <td style="width: 10px;"></td>
                        </tr>

                        <tr>
                            <td style="width: 5px;"></td>

                            <td style="vertical-align: central;">
                                <div>
                                    <input class="Input" type="text" placeholder="Please enter your name" />
                                </div>
                            </td>

                            <td style="width: 10px;"></td>
                        </tr>
                    </table>
                </td>
            </tr>



            <tr style="height: 1px;">
                <td>
                    <div style="margin-top: 9px; float: right">
                        <button class="RedButton" onclick="CloseButtonClicked()" style="float: right; margin-left: 7px; margin-right: 7px;" title="Close">Close</button>
                    </div>
                    <div style="margin-left: 3px; margin-top: 9px; float: right">
                        <button class="GreenButton" onclick="ApproveButtonClicked()" style="float: right; margin-left: 7px;" title="Approve">Approve</button>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        function CloseButtonClicked() {
            window.close();
        }
        function ApproveButtonClicked() {
            window.CloseEvent();
        }

    </script>

    <script type="text/javascript" src="DocumentsApprovalPageViewModel.js"></script>

    <style>
        .GreenButton {
            border: 1px solid #009161;
            background: -moz-linear-gradient(50% 0% -90deg,rgba(255, 255, 255, 1) 0%,rgba(0, 145, 97, 0.6) 100%);
            background: -webkit-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(0, 145, 97, 0.6) 100%);
            background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(255, 255, 255, 1) ),color-stop(1,rgba(0, 145, 97, 0.6) ));
            background: -o-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(0, 145, 97, 0.6) 100%);
            background: linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgba(0, 145, 97, 0.6) 100%);
        }

        .RedButton {
            background: -moz-linear-gradient(50% 0% -90deg,rgba(255, 255, 255, 1) 0%,rgba(237, 192, 147, 1) 100%) !important;
            background: -webkit-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(237, 192, 147, 1) 100%) !important;
            background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(255, 255, 255, 1) ),color-stop(1,rgba(237, 192, 147, 1) )) !important;
            background: -o-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(237, 192, 147, 1) 100%) !important;
            background: linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgba(237, 192, 147, 1) 100%) !important;
        }

        .Button {
            background: -moz-linear-gradient(50% 0% -90deg,rgba(255, 255, 255, 1) 0%,rgba(186, 206, 227, 1) 100%);
            background: -webkit-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(186, 206, 227, 1) 100%);
            background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(255, 255, 255, 1) ),color-stop(1,rgba(186, 206, 227, 1) ));
            background: -o-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(186, 206, 227, 1) 100%);
            background: linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgba(186, 206, 227, 1) 100%);
        }

        .Button, .RedButton, .GreenButton {
            /*display: block;*/
            outline: none;
            text-align: center;
            font-size: 11px;
            color: #45494A;
            /*width: 100%;*/
            height: 22px;
            border: 1px solid #6A8299;
            border-radius: 3px;
            -moz-border-radius: 3px;
            -webkit-border-radius: 3px;
            text-shadow: 1px 1px white;
            position: relative;
            cursor: pointer;
        }

        .LogitudeWindow {
            position: absolute;
            border-radius: 8px;
            -moz-border-radius: 8px;
            -webkit-border-radius: 8px;
            background: -moz-linear-gradient(50% 100% 90deg,rgba(255, 255, 255, 1) 89.25%,rgba(230, 230, 230, 1) 100%);
            background: -webkit-linear-gradient(90deg, rgba(255, 255, 255, 1) 89.25%, rgba(230, 230, 230, 1) 100%);
            background: -o-linear-gradient(90deg, rgba(255, 255, 255, 1) 89.25%, rgba(230, 230, 230, 1) 100%);
            background: linear-gradient(0deg, rgba(255, 255, 255, 1) 89.25%, rgba(230, 230, 230, 1) 100%);
            border: 1px solid #C8C8C8;
            overflow: hidden;
        }

        .Input {
            height: 22px;
            min-height: 22px;
            max-height: 22px;
            /*line-height: 21px;*/
            width: 100%;
            border: 1px solid #AAAAAA;
            outline: none;
            font-size: 11px;
            color: #45494A;
            background: white;
            /*text-indent: 5px;*/
            border-radius: 3px;
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
            -moz-box-shadow: inset 0 0 3px #AAAAAA;
            -webkit-box-shadow: inset 0 0 3px #AAAAAA;
            box-shadow: inset 0 0 3px #AAAAAA;
        }

            .Input:hover {
                border: 1px solid #3BB3E2;
            }
    </style>

</body>
</html>
