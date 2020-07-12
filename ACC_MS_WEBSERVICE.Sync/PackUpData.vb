Imports ACC_MS_WEBSERVICE.PHASE3
Public Class PackUpData
    Public Function ManualPackUpData(ByVal refNumber As String, ByVal accountNumber As String) As RequestResponse
        Dim DAL As New DAL
        Dim myTrans As New List(Of System.Data.SqlClient.SqlTransaction)
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name


        Dim MemberContactinfo As New MemberContactinfo
        Dim MembershipCategoryInfo As New MembershipCategoryInfo
        Dim DCS_Card_Account As New DCS_Card_Account
        Dim Photo As New Photo
        Dim Member As New Member
        Dim Signature As New Signature
        Dim PhotoValidID As New PhotoValidID
        Dim Bio As New Bio

        Dim requestAuth As New RequestAuth
        Dim response As New RequestResponse


        If MemberContactinfo.Load(DAL, refNumber) Then
            If MembershipCategoryInfo.Load(DAL, refNumber) Then
                If DCS_Card_Account.Load(DAL, refNumber) Then
                    If Member.Load(DAL, refNumber) Then
                        If Photo.Load(DAL, refNumber) Then
                            If Signature.Load(DAL, refNumber) Then
                                If PhotoValidID.Load(DAL, refNumber) Then
                                    If Bio.Load(DAL, refNumber) Then
                                        response.IsSuccess = True
                                    Else
                                        response.IsSuccess = False
                                        response.ErrorMessage = Bio.ErrorMessage
                                    End If
                                Else
                                    response.IsSuccess = False
                                    response.ErrorMessage = PhotoValidID.ErrorMessage
                                End If
                            Else
                                response.IsSuccess = False
                                response.ErrorMessage = Signature.ErrorMessage
                            End If
                        Else
                            response.IsSuccess = False
                            response.ErrorMessage = Photo.ErrorMessage
                        End If
                    Else
                        response.IsSuccess = False
                        response.ErrorMessage = Member.ErrorMessage
                    End If
                Else
                    response.IsSuccess = False
                    response.ErrorMessage = DCS_Card_Account.ErrorMessage
                End If
            Else
                response.IsSuccess = False
                response.ErrorMessage = MembershipCategoryInfo.ErrorMessage
            End If
        Else
            response.IsSuccess = False
            response.ErrorMessage = MemberContactinfo.ErrorMessage
        End If

        If response.IsSuccess = True Then
            If My.Settings.BankID = "1" Then
                If Not accountNumber = "" Then
                    Dim account = accountNumber.Substring(accountNumber.Length - 4, 4)
                    Dim dbaccount = DCS_Card_Account.AccountNumber.Substring(DCS_Card_Account.AccountNumber.Length - 4, 4)
                    If account = dbaccount Then
                        DCS_Card_Account.AccountNumber = accountNumber
                    End If
                End If
                Try
                    Dim reqBranch As String = ""
                    Dim reqBranchAddress As String = ""

                    If DAL.SelectQuery("SELECT ISNULL(HBUR,'') As HBUR, ISNULL(Building,'') As Building, ISNULL(LotNo,'') As LotNo, ISNULL(BlockNo,'') As BlockNo, ISNULL(PhaseNo,'') As PhaseNo, ISNULL(HouseNo,'') As HouseNo, ISNULL(StreetName,'') As StreetName, ISNULL(Subdivision,'') As Subdivision, ISNULL(Barangay,'') As Barangay, ISNULL(CityMinicipality,'') As CityMinicipality, ISNULL(Province,'') As Province, ISNULL(ZipCode,'') As ZipCode, ISNULL(Region,'') As Region, ISNULL(Branch,'') As Branch FROM tbl_branch WHERE requesting_branchcode='" & Member.requesting_branchcode & "'") Then
                        If DAL.TableResult.DefaultView.Count > 0 Then
                            reqBranch = DAL.TableResult.Rows(0)("Branch").ToString
                            reqBranchAddress = DAL.TableResult.Rows(0)("HBUR").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("Building").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("LotNo").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("BlockNo").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("PhaseNo").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("HouseNo").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("StreetName").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("Subdivision").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("Barangay").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("CityMinicipality").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("Province").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("Region").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("ZipCode").ToString
                        End If
                    End If

                    Dim arrData As String = ""
                    Dim permanentAddress As String = Trim(MemberContactinfo.Permanent_HBUR & " " & MemberContactinfo.Permanent_Building & " " & MemberContactinfo.Permanent_LotNo & " " & MemberContactinfo.Permanent_BlockNo & " " & MemberContactinfo.Permanent_PhaseNo & " " & MemberContactinfo.Permanent_HouseNo & " " & MemberContactinfo.Permanent_StreetName & " " & MemberContactinfo.Permanent_Subdivision & " " & MemberContactinfo.Permanent_Barangay & " " & MemberContactinfo.Permanent_CityMunicipality & " " & MemberContactinfo.Permanent_Province & " " & MemberContactinfo.Permanent_Region & " " & MemberContactinfo.Permanent_ZipCode)
                    Dim presentAddress As String = Trim(MemberContactinfo.Present_HBUR & " " & MemberContactinfo.Present_Building & " " & MemberContactinfo.Present_LotNo & " " & MemberContactinfo.Present_BlockNo & " " & MemberContactinfo.Present_PhaseNo & " " & MemberContactinfo.Present_HouseNo & " " & MemberContactinfo.Present_StreetName & " " & MemberContactinfo.Present_Subdivision & " " & MemberContactinfo.Present_Barangay & " " & MemberContactinfo.Present_CityMunicipality & " " & MemberContactinfo.Present_Province & " " & MemberContactinfo.Present_Region & " " & MemberContactinfo.Present_ZipCode)
                    Dim employerAddress As String = Trim(MembershipCategoryInfo.Employer_HBUR & " " & MembershipCategoryInfo.Employer_Building & " " & MembershipCategoryInfo.Employer_LotNo & " " & MembershipCategoryInfo.Employer_BlockNo & " " & MembershipCategoryInfo.Employer_PhaseNo & " " & MembershipCategoryInfo.Employer_HouseNo & " " & MembershipCategoryInfo.Employer_StreetName & " " & MembershipCategoryInfo.Employer_Subdivision & " " & MembershipCategoryInfo.Employer_Barangay & " " & MembershipCategoryInfo.Employer_CityMunicipality & " " & MembershipCategoryInfo.Employer_Province & " " & MembershipCategoryInfo.Employer_Region & " " & MembershipCategoryInfo.Employer_ZipCode)

                    permanentAddress = permanentAddress.Replace(",", " ").Replace(".", " ").Replace("#", " ").Replace("-", " ").Replace("(", " ").Replace(")", " ").Replace("&", " ").Replace("'", " ").Replace("""", " ").Replace("+", " ")
                    presentAddress = presentAddress.Replace(",", " ").Replace(".", " ").Replace("#", " ").Replace("-", " ").Replace("(", " ").Replace(")", " ").Replace("&", " ").Replace("'", " ").Replace("""", " ").Replace("+", " ")
                    employerAddress = employerAddress.Replace(",", " ").Replace(".", " ").Replace("#", " ").Replace("-", " ").Replace("(", " ").Replace(")", " ").Replace("&", " ").Replace("'", " ").Replace("""", " ").Replace("+", " ")

                    ' Handling for MID
                    Dim sssID = Member.SSSID
                    Dim gsisID = Member.GSISID
                    If sssID = "" And gsisID IsNot "" Then
                        sssID = Member.PagIBIGID
                    ElseIf sssID IsNot "" And gsisID = "" Then
                        gsisID = Member.PagIBIGID
                    ElseIf sssID IsNot "" And gsisID IsNot "" Then
                        gsisID = Member.PagIBIGID
                    End If
                    ' Handling for recarding.
                    Dim employeeID = MembershipCategoryInfo.EmployeeID
                    Dim _accountNumber = DCS_Card_Account.AccountNumber
                    If Member.Application_Remarks.Contains("Re-card") Then
                        employeeID = "" ' Should replace by old card number.
                        _accountNumber = DCS_Card_Account.CardNo
                    End If

                    arrData =
                                                            _accountNumber & "|" &
                                                            Member.Member_LastName & "|" &
                                                            Member.Member_FirstName & "|" &
                                                            Member.Member_MiddleName & "|" &
                                                             "P|" &
                                                             CDate(Member.BirthDate).ToString("MM/dd/yyyy") & "|" &
                                                             Member.Gender.Substring(0, 1).ToUpper & "|" &
                                                             Member.CivilStatus.Substring(0, 1).ToUpper & "|" &
                                                             (MemberContactinfo.Mobile_AreaCode & MemberContactinfo.Mobile_CelNo).Replace("+", "").Replace(" ", "") & "|" &
                                                             (MemberContactinfo.Home_CountryCode & " " & MemberContactinfo.Home_AreaCode & " " & MemberContactinfo.Home_TelNo) & "|" &
                                                             MemberContactinfo.EmailAddress & "|" &
                                                             Member.TIN.ToUpper & "|" &
                                                             permanentAddress & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             reqBranch.Trim & "|" &
                                                             reqBranchAddress.Trim & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             "0RCKAGALINGAN" & "|" &
                                                             "PI1" & "|" &
                                                             Now.ToString("MM/dd/yyyy") & "|" &
                                                             "" & "|" &
                                                             Member.BirthCity & "|" &
                                                             "Filipino" & "|" &
                                                             "H1000" & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             sssID & "|" &
                                                             gsisID & "|" &
                                                             "" & "|" &
                                                             employeeID & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             MembershipCategoryInfo.EmployerName.Trim & "|" &
                                                             employerAddress.Trim & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             "|||||||||||"

                    arrData = arrData.Replace("ñ", "n")
                    arrData = arrData.Replace("Ñ", "N")

                    PHASE3.PHASE3.PackUBPData(requestAuth, Trim(arrData), Member.PagIBIGID, DCS_Card_Account.AccountNumber,
                                                                                      Photo.fld_Photo, Signature.fld_Signature, PhotoValidID.fld_PhotoID,
                                                                                      Bio.fld_LeftPrimaryFP_Ansi, Bio.fld_LeftSecondaryFP_Ansi, Bio.fld_RightPrimaryFP_Ansi, Bio.fld_RightSecondaryFP_Ansi,
                                                                                      Bio.fld_LeftPrimaryFP_Wsq, Bio.fld_LeftSecondaryFP_Wsq, Bio.fld_RightPrimaryFP_Wsq, Bio.fld_RightSecondaryFP_Wsq, Member.Application_Remarks.Contains("Re-card"))

                    Dim sessionRefTxn As String = GenerateSessionRefTxn(requestAuth)

                    If DAL.AddUBP(requestAuth.User, requestAuth.KioskID, sessionRefTxn, Member.PagIBIGID, DCS_Card_Account.AccountNumber) Then
                    End If
                    response.IsSuccess = True
                Catch ex As Exception
                    response.IsSuccess = False
                    response.ErrorMessage = ex.Message
                    PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Packing of ubp data. Runtime error " & ex.Message)
                End Try
            ElseIf My.Settings.BankID = "2" Then
                Try
                    PHASE3.PHASE3.PackUBPData2(requestAuth, Member.PagIBIGID,
                                                                                      Photo.fld_Photo, Signature.fld_Signature, PhotoValidID.fld_PhotoID,
                                                                                      Bio.fld_LeftPrimaryFP_Ansi, Bio.fld_LeftSecondaryFP_Ansi, Bio.fld_RightPrimaryFP_Ansi, Bio.fld_RightSecondaryFP_Ansi,
                                                                                      Bio.fld_LeftPrimaryFP_Wsq, Bio.fld_LeftSecondaryFP_Wsq, Bio.fld_RightPrimaryFP_Wsq, Bio.fld_RightSecondaryFP_Wsq, Member.ApplicationDate)

                    Dim sessionRefTxn As String = GenerateSessionRefTxn(requestAuth)


                    response.IsSuccess = True
                Catch ex As Exception
                    response.IsSuccess = False
                    response.ErrorMessage = ex.Message
                    PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Packing of ubp data. Runtime error " & ex.Message)
                End Try

            End If
        End If
        Return response
    End Function

    Private Function GenerateSessionRefTxn(ByVal requestAuth As PHASE3.RequestAuth) As String
        Dim rn As New Random
        Return String.Format("{0}{1}{2}{3}", requestAuth.User.ToString.PadLeft(3, "0"), requestAuth.KioskID, Now.ToString("yyyyMMddhhmmss"), rn.Next(1000, 9999))
    End Function
End Class
