
Imports System.IO
Imports System.Net
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates

Namespace PHASE3

    Public Class PHASE3

        Private Shared TIME_KEEPING_LINK As String = My.Settings.TimeKeeping_BasedURL
        Private Shared API_LINK As String = My.Settings.PAGIBIG_BasedURL
        Private Shared USER_ID As String = My.Settings.PAGIBIG_WS_User
        Private Shared USER_PASS As String = My.Settings.PAGIBIG_WS_Pass

        Public Shared WS_AuthFailedMsg As String = "Authentication failed for webservice function"
        Public Shared WS_RequestAuthIsNothingMsg As String = "Request authentication is nothing"
        Private Shared encryptionKey As String = "ragMANOK2kx2014"


#Region " GetMemberInfo "

        Private pSearchResult As SearchResult
        Public Property SearchResult As SearchResult
            Get
                Return pSearchResult
            End Get
            Set(ByVal value As SearchResult)
                pSearchResult = value
            End Set
        End Property

        Public Function GetMemberInfo(ByVal mid As String, ByRef response As String) As Boolean
            Try

                'Dim client = New PagibigWebService.Service()
                'client.Url = API_LINK
                'Dim result = client.GetMemberInfo(USER_ID, Decrypt(USER_PASS), mid)
                'If result.IsGet Then
                '    PopulateMemberInfo(result)
                '    Return True
                'Else
                '    Return False
                '    response = result.GetErrorMsg()
                'End If


                Dim SoapResponse As String = ""
                Dim ErrMsg As String = ""

                If Execute_WSDL(API_LINK, SoapRequest("GetMemberInfo", New String() {"UID", "PWD", "MEMBERID"}, New String() {USER_ID, Decrypt(USER_PASS), mid}), SoapResponse, ErrMsg) Then
                    'If Execute_WSDL(API_LINK, SoapRequest("GetMemberInfo", New String() {"UID", "PWD", "MEMBERID"}, New String() {USER_ID, USER_PASS, mid}), SoapResponse, ErrMsg) Then
                    PopulateMemberInfo(SoapResponse)
                    Return True
                Else
                    response = ErrMsg
                    'Return FailResponse
                    Return False
                End If

            Catch ex As Exception
                response = ex.Message
                Return False
            Finally
            End Try
        End Function
        Private Sub PopulateMemberInfo(ByVal result As PagibigWebService.SearchResult)

            Dim member As New MemberInfo
            member.MemberID = result.MemberInfo.PagIBIGIDNo

            member.MemberName = New MemberName
            member.MemberName = PopulateName(result.MemberInfo.MemberName)
            member.MotherName = New MotherName
            member.MotherName = PopulateName(result.MemberInfo.MotherName)
            member.FatherName = New FatherName
            member.FatherName = PopulateName(result.MemberInfo.FatherName)
            member.SpouseName = New SpouseName
            member.SpouseName = PopulateName(result.MemberInfo.SpouseName)

            member.MemberBirthdate = result.MemberInfo.DateOfBirth
            member.MemberPlaceOfBirth = result.MemberInfo.PlaceOfBirth
            member.MemberBirthPlaceCountry = result.MemberInfo.CountryOfBirth
            member.MemberBirthPlaceMunicipal = result.MemberInfo.PlaceOfBirth
            member.MemberGender = result.MemberInfo.Gender
            member.MemberCivilStatus = result.MemberInfo.CivilStatus
            member.MemberCitizenshipCode = result.MemberInfo.Citizenship.Code
            member.MemberCitizenship = result.MemberInfo.Citizenship.Description

            member.MemberEmail = result.MemberInfo.EmailAddress
            member.MemberCRN = result.MemberInfo.CommonRefNo
            member.MemberSSS = result.MemberInfo.SSSIDNo
            member.MemberGSIS = result.MemberInfo.GSISIDNo
            member.MemberTIN = result.MemberInfo.TIN
            member.MembershipCategory = New MembershipCategory
            member.MembershipCategory = PopulateMemberCategoryInfo(result.MemberInfo.MemberCategoryInfo)
            member.PermanentAddress = New PermanentAddress
            member.PermanentAddress = PopulateAddress(result.MemberInfo.PermanentAddress)
            member.EmploymentHistoryList = New EmploymentHistoryList
            member.EmploymentHistoryList = PopulateEmploymentHistory(result.MemberInfo.EmploymentHistory)
            member.PresentAddress = New PresentAddress
            member.PresentAddress = PopulateAddress(result.MemberInfo.PresentAddress)
            member.HomeTelNo = New HomeTelNo
            member.HomeTelNo = PopulateContactInfo(result.MemberInfo.HomeTelNo)
            member.MobileTelNo = New MobileTelNo
            member.MobileTelNo = PopulateContactInfo(result.MemberInfo.MobileTelNo)

            member.BusinessDirectTelNo = PopulateContactInfo(result.MemberInfo.BusinessDirectTelNo)
            member.BusinessTrunkTelNo = PopulateContactInfo(result.MemberInfo.BusinessTrunkTelNo)

            pSearchResult = New SearchResult
            pSearchResult.IsGet = result.IsGet
            pSearchResult.IsComplete = result.IsComplete
            pSearchResult.GetErrorMsg = result.GetErrorMsg
            pSearchResult.MemberInfo = member

        End Sub
        Private Function PopulateName(ByVal result As PagibigWebService.HDMFName) As MemberName
            Dim fName As New MemberName
            fName.LastName = result.LastName
            fName.FirstName = result.FirstName
            fName.MiddleName = result.MiddleName
            fName.Ext = result.NameExtension
            fName.IsNoMiddleName = result.NoMiddleName
            Return fName
        End Function
        Private Function PopulateAddress(ByVal result As PagibigWebService.HDMFAddress) As PermanentAddress
            Dim DAL As New DAL
            Dim address As New PermanentAddress
            address.HBURNumber = result.HBURNumber
            address.Building = result.Building
            address.LotNo = result.Lot_No
            address.BlockNo = result.Block_No
            address.PhaseNo = result.Phase_No
            address.HouseNo = result.House_No
            address.Barangay = result.Baranggay
            address.BarangayCode = GetAddressCode(DAL, String.Format("SELECT psgc_barangay_code FROM tbl_Ref_City_Municipality WHERE psgc_barangay_desc='{0}'", result.Baranggay))
            address.StreetName = result.StreetName
            address.Subdivision = result.Subdivision
            address.CityMunicipality = result.CityMunicipality
            address.CityMunicipalityCode = GetAddressCode(DAL, String.Format("SELECT psgc_city_mun_code FROM tbl_Ref_City_Municipality WHERE psgc_city_mun_desc='{0}'", result.CityMunicipality))
            address.Province = result.Province
            address.ProvinceCode = GetAddressCode(DAL, String.Format("SELECT psgc_region_code FROM tbl_Ref_Region WHERE psgc_region_desc='{0}'", result.Province))
            address.Region = result.Region
            address.RegionCode = GetAddressCode(DAL, String.Format("SELECT psgc_region_code FROM tbl_Ref_Region WHERE psgc_region_desc='{0}'", result.Region))
            address.Zipcode = result.ZIP_Code

            Return address
        End Function
        Private Function PopulateMemberCategoryInfo(ByVal result As PagibigWebService.HDMFMembershipCategory) As MembershipCategory
            Dim category As New MembershipCategory
            category.EmployerCategory = result.MembershipCategory
            category.EmployerName = result.EmployerName
            category.EmployerAddress = PopulateAddress(result.EmployerAddress)
            category.EmployeeID = result.EmployeeID
            category.DateEmployed = result.DateEmployed
            category.EmploymentStatus = result.EmploymentStatus
            category.OccupationCode = result.Occupation.Code
            category.Occupation = result.Occupation.Description
            category.AFPSerialBadgeNo = result.AFPSerialBadgeNo
            category.DepEdDivCodeStnCode = result.DepEdDivCodeStnCode
            category.TypeOfWork = result.TypeOfWork
            category.ManningAgency = result.ManningAgency
            category.CountryOfAssignment = result.CountryofAssignment
            category.MonthlyIncome = result.BasicSalary
            Return category
        End Function
        Private Function PopulateEmploymentHistory(ByVal result As PagibigWebService.HDMFEmploymentHistory()) As EmploymentHistoryList

            Dim empList As New EmploymentHistoryList
            Dim employemenyHistory As New EmploymentHistory

            For Each emp As PagibigWebService.HDMFEmploymentHistory In result
                employemenyHistory.EmployerName = emp.EmployerName
                employemenyHistory.EmployerAddress = emp.EmployerAddress
                employemenyHistory.DateEmployed = emp.DateEmployed
                employemenyHistory.DateSeparated = emp.DateSeparated
            Next
            empList.EmploymentHistoryList.Add(employemenyHistory)
            Return empList
        End Function
        Private Function PopulateContactInfo(ByVal result As PagibigWebService.HDMFContactInfo) As HomeTelNo
            Dim contactInfo As HomeTelNo
            contactInfo.CountryCode = result.Country_Code
            contactInfo.AreaCode = result.Area_Code
            contactInfo.TelNo = result.Tel_No
            contactInfo.LocalNo = result.Local_No
            Return contactInfo
        End Function

        Private Sub PopulateMemberInfo(ByVal responsebody As String)
            Dim x As System.Xml.XmlDocument = New System.Xml.XmlDocument()
            x.LoadXml(responsebody)
            Dim list As System.Xml.XmlNodeList = x.GetElementsByTagName("GetMemberInfoResult")

            pSearchResult = New SearchResult

            For Each cnNode1 As System.Xml.XmlNode In list

                For Each cnNode2 As System.Xml.XmlNode In cnNode1.ChildNodes

                    Select Case cnNode2.Name
                        Case "IsGet"
                            pSearchResult.IsGet = Convert.ToBoolean(cnNode2.InnerText)
                        Case "MemberInfo"
                            Dim member As MemberInfo = New MemberInfo
                            PopulateMember(responsebody, member)
                            pSearchResult.MemberInfo = member
                        Case "IsComplete"
                            pSearchResult.IsComplete = Convert.ToBoolean(cnNode2.InnerText)
                        Case "GetErrorMsg"
                            pSearchResult.GetErrorMsg = cnNode2.InnerText
                    End Select
                Next
            Next
        End Sub

        Private Shared Sub PopulateMember(ByVal responsebody As String, ByRef member As MemberInfo)
            Dim x As System.Xml.XmlDocument = New System.Xml.XmlDocument()
            x.LoadXml(responsebody)
            Dim list As System.Xml.XmlNodeList = x.GetElementsByTagName("MemberInfo")

            For Each cnNode1 As System.Xml.XmlNode In list

                For Each cnNode2 As System.Xml.XmlNode In cnNode1.ChildNodes

                    Select Case cnNode2.Name
                        Case "PagIBIGIDNo"
                            member.MemberID = cnNode2.InnerText
                        Case "MemberName"
                            Dim memberName As New MemberName
                            PopulateName(responsebody, "MemberName", memberName)
                            member.MemberName = memberName
                        Case "MotherName"
                            Dim motherName As New MotherName
                            PopulateName(responsebody, "MotherName", motherName)
                            member.MotherName = motherName
                        Case "FatherName"
                            Dim fatherName As New FatherName
                            PopulateName(responsebody, "FatherName", fatherName)
                            member.FatherName = fatherName
                        Case "SpouseName"
                            Dim spouseName As New SpouseName
                            PopulateName(responsebody, "SpouseName", spouseName)
                            member.SpouseName = spouseName
                        Case "DateOfBirth"
                            member.MemberBirthdate = cnNode2.InnerText
                        Case "PlaceOfBirth"
                            member.MemberPlaceOfBirth = cnNode2.InnerText
                        Case "CountryOfBirth"
                            member.MemberBirthPlaceCountry = cnNode2.InnerText
                        Case "CityMunicipalityOfBirth"
                            member.MemberBirthPlaceMunicipal = cnNode2.InnerText
                        Case "Gender"
                            member.MemberGender = cnNode2.InnerText
                        Case "CivilStatus"
                            member.MemberCivilStatus = cnNode2.InnerText
                        Case "Citizenship"
                            Dim descCode As New HDMFDescCode
                            PopulateDescCode(responsebody, "Citizenship", descCode)
                            member.MemberCitizenshipCode = descCode.Code
                            member.MemberCitizenship = descCode.Description
                        Case "EmailAddress"
                            member.MemberEmail = cnNode2.InnerText
                        Case "CommonRefNo"
                            member.MemberCRN = cnNode2.InnerText
                        Case "SSSIDNo"
                            member.MemberSSS = cnNode2.InnerText
                        Case "GSISIDNo"
                            member.MemberGSIS = cnNode2.InnerText
                        Case "TIN"
                            member.MemberTIN = cnNode2.InnerText
                        Case "MemberCategoryInfo"
                            Dim membershipCategory As New MembershipCategory
                            PopulateMemberCategoryInfo(responsebody, membershipCategory)
                            member.MembershipCategory = membershipCategory
                        Case "PermanentAddress"
                            Dim permanentAddress As New PermanentAddress
                            PopulateAddress(responsebody, "PermanentAddress", permanentAddress)
                            member.PermanentAddress = permanentAddress
                        Case "EmploymentHistory"
                            Dim employmentHistoryList As New EmploymentHistoryList
                            employmentHistoryList.EmploymentHistoryList = New List(Of EmploymentHistory)
                            PopulateEmploymentHistory(responsebody, "HDMFEmploymentHistory", employmentHistoryList)
                            member.EmploymentHistoryList = employmentHistoryList
                        Case "PresentAddress"
                            Dim presentAddress As New PresentAddress
                            PopulateAddress(responsebody, "PresentAddress", presentAddress)
                            member.PresentAddress = presentAddress
                        Case "HomeTelNo"
                            Dim homeTelNo As New HomeTelNo
                            PopulateContactInfo(responsebody, "HomeTelNo", homeTelNo)
                            member.HomeTelNo = homeTelNo
                        Case "MobileTelNo"
                            Dim mobileTelNo As New MobileTelNo
                            PopulateContactInfo(responsebody, "MobileTelNo", mobileTelNo)
                            mobileTelNo.CountryCode = IIf(Not mobileTelNo.CountryCode = "", "+63", mobileTelNo.CountryCode)
                            member.MobileTelNo = mobileTelNo
                        Case "BusinessDirectTelNo"
                            Dim businessDirectTelNo As New BusinessDirectTelNo
                            PopulateContactInfo(responsebody, "BusinessDirectTelNo", businessDirectTelNo)
                            member.BusinessDirectTelNo = businessDirectTelNo
                        Case "BusinessTrunkTelNo"
                            Dim businessTrunkTelNo As New BusinessTrunkTelNo
                            PopulateContactInfo(responsebody, "BusinessTrunkTelNo", businessTrunkTelNo)
                            member.BusinessTrunkTelNo = businessTrunkTelNo
                    End Select
                Next
            Next
        End Sub

        Private Shared Sub PopulateName(ByVal responsebody As String, ByVal xmlTag As String, ByRef fName As MemberName)
            Dim x As System.Xml.XmlDocument = New System.Xml.XmlDocument()
            x.LoadXml(responsebody)
            Dim list As System.Xml.XmlNodeList = x.GetElementsByTagName(xmlTag)

            For Each cnNode1 As System.Xml.XmlNode In list

                For Each cnNode2 As System.Xml.XmlNode In cnNode1.ChildNodes

                    Select Case cnNode2.Name
                        Case "LastName"
                            fName.LastName = cnNode2.InnerText
                        Case "FirstName"
                            fName.FirstName = cnNode2.InnerText
                        Case "MiddleName"
                            fName.MiddleName = cnNode2.InnerText
                        Case "NameExtension"
                            fName.Ext = cnNode2.InnerText
                        Case "NoMiddleName"
                            fName.IsNoMiddleName = Convert.ToBoolean(cnNode2.InnerText)
                    End Select
                Next
            Next
        End Sub

        Private Shared Sub PopulateDescCode(ByVal responsebody As String, ByVal xmlTag As String, ByRef descCode As HDMFDescCode)
            Dim x As System.Xml.XmlDocument = New System.Xml.XmlDocument()
            x.LoadXml(responsebody)
            Dim list As System.Xml.XmlNodeList = x.GetElementsByTagName(xmlTag)

            For Each cnNode1 As System.Xml.XmlNode In list

                For Each cnNode2 As System.Xml.XmlNode In cnNode1.ChildNodes

                    Select Case cnNode2.Name
                        Case "Description"
                            descCode.Description = cnNode2.InnerText
                        Case "Code"
                            descCode.Code = cnNode2.InnerText
                    End Select
                Next
            Next
        End Sub

        Private Shared Sub PopulateEmploymentHistory(ByVal responsebody As String, ByVal xmlTag As String, ByRef employmentHistoryList As EmploymentHistoryList)
            Dim x As System.Xml.XmlDocument = New System.Xml.XmlDocument()
            x.LoadXml(responsebody)
            Dim list As System.Xml.XmlNodeList = x.GetElementsByTagName(xmlTag)

            For Each cnNode1 As System.Xml.XmlNode In list
                Dim employmentHistory As New EmploymentHistory

                For Each cnNode2 As System.Xml.XmlNode In cnNode1.ChildNodes
                    Select Case cnNode2.Name
                        Case "EmployerName"
                            employmentHistory.EmployerName = cnNode2.InnerText
                        Case "EmployerAddress"
                            employmentHistory.EmployerAddress = cnNode2.InnerText
                        Case "DateEmployed"
                            employmentHistory.DateEmployed = cnNode2.InnerText
                        Case "DateSeparated"
                            employmentHistory.DateSeparated = cnNode2.InnerText
                    End Select
                Next

                If employmentHistory.EmployerName <> "" Then employmentHistoryList.EmploymentHistoryList.Add(employmentHistory)
            Next
        End Sub

        Private Shared Sub PopulateAddress(ByVal responsebody As String, ByVal xmlTag As String, ByRef address As PermanentAddress)
            Dim x As System.Xml.XmlDocument = New System.Xml.XmlDocument()
            x.LoadXml(responsebody)
            Dim list As System.Xml.XmlNodeList = x.GetElementsByTagName(xmlTag)

            Dim DAL As New DAL

            For Each cnNode1 As System.Xml.XmlNode In list

                For Each cnNode2 As System.Xml.XmlNode In cnNode1.ChildNodes

                    Select Case cnNode2.Name
                        Case "HBURNumber"
                            address.HBURNumber = cnNode2.InnerText
                        Case "Building"
                            address.Building = cnNode2.InnerText
                        Case "Lot_No"
                            address.LotNo = cnNode2.InnerText
                        Case "Block_No"
                            address.BlockNo = cnNode2.InnerText
                        Case "Phase_No"
                            address.PhaseNo = cnNode2.InnerText
                        Case "House_No"
                            address.HouseNo = cnNode2.InnerText
                        Case "Baranggay"
                            address.Barangay = cnNode2.InnerText
                            address.BarangayCode = GetAddressCode(DAL, String.Format("SELECT psgc_barangay_code FROM tbl_Ref_City_Municipality WHERE psgc_barangay_desc='{0}'", address.Barangay))
                        Case "StreetName"
                            address.StreetName = cnNode2.InnerText
                        Case "Subdivision"
                            address.Subdivision = cnNode2.InnerText
                        Case "CityMunicipality"
                            address.CityMunicipality = cnNode2.InnerText
                            address.CityMunicipalityCode = GetAddressCode(DAL, String.Format("SELECT psgc_city_mun_code FROM tbl_Ref_City_Municipality WHERE psgc_city_mun_desc='{0}'", address.CityMunicipality))
                        Case "Province"
                            address.Province = cnNode2.InnerText
                            address.ProvinceCode = GetAddressCode(DAL, String.Format("SELECT psgc_region_code FROM tbl_Ref_Region WHERE psgc_region_desc='{0}'", address.Province))
                        Case "Region"
                            address.Region = cnNode2.InnerText
                            address.RegionCode = GetAddressCode(DAL, String.Format("SELECT psgc_region_code FROM tbl_Ref_Region WHERE psgc_region_desc='{0}'", address.Region))
                        Case "ZIP_Code"
                            address.Zipcode = cnNode2.InnerText
                    End Select
                Next
            Next

            DAL.Dispose()
            DAL = Nothing
        End Sub

        Private Shared Function GetAddressCode(ByVal DAL As DAL, ByVal strQuery As String) As String
            If DAL.ExecuteScalar(strQuery) Then
                If Not DAL.ObjectResult Is Nothing Then _
                    If Not IsDBNull(DAL.ObjectResult) Then _
                        Return DAL.ObjectResult.ToString
            End If

            Return ""
        End Function

        Private Shared Sub PopulateMemberCategoryInfo(ByVal responsebody As String, ByRef membershipCategory As MembershipCategory)
            Dim x As System.Xml.XmlDocument = New System.Xml.XmlDocument()
            x.LoadXml(responsebody)
            Dim list As System.Xml.XmlNodeList = x.GetElementsByTagName("MemberCategoryInfo")

            For Each cnNode1 As System.Xml.XmlNode In list

                For Each cnNode2 As System.Xml.XmlNode In cnNode1.ChildNodes

                    Select Case cnNode2.Name
                        Case "MembershipCategory"
                            membershipCategory.EmployerCategory = cnNode2.InnerText
                        Case "EmployerName"
                            membershipCategory.EmployerName = cnNode2.InnerText
                        Case "EmployerAddress"
                            Dim employerAddress As New EmployerAddress
                            PopulateAddress(responsebody, "EmployerAddress", employerAddress)
                            membershipCategory.EmployerAddress = employerAddress
                        Case "EmployeeID"
                            membershipCategory.EmployeeID = cnNode2.InnerText
                        Case "DateEmployed"
                            membershipCategory.DateEmployed = cnNode2.InnerText
                        Case "EmploymentStatus"
                            membershipCategory.EmploymentStatus = cnNode2.InnerText
                        Case "Occupation"
                            Dim descCode As New HDMFDescCode
                            PopulateDescCode(responsebody, "Occupation", descCode)
                            membershipCategory.OccupationCode = descCode.Code
                            membershipCategory.Occupation = descCode.Description
                        Case "AFPSerialBadgeNo"
                            membershipCategory.AFPSerialBadgeNo = cnNode2.InnerText
                        Case "DepEdDivCodeStnCode"
                            membershipCategory.DepEdDivCodeStnCode = cnNode2.InnerText
                        Case "TypeOfWork"
                            membershipCategory.TypeOfWork = cnNode2.InnerText
                        Case "ManningAgency"
                            membershipCategory.ManningAgency = cnNode2.InnerText
                        Case "CountryofAssignment"
                            membershipCategory.CountryOfAssignment = cnNode2.InnerText
                        Case "BasicSalary"
                            membershipCategory.MonthlyIncome = cnNode2.InnerText
                    End Select
                Next
            Next
        End Sub

        Private Shared Sub PopulateContactInfo(ByVal responsebody As String, ByVal xmlTag As String, ByRef contactInfo As HomeTelNo)
            Dim x As System.Xml.XmlDocument = New System.Xml.XmlDocument()
            x.LoadXml(responsebody)
            Dim list As System.Xml.XmlNodeList = x.GetElementsByTagName(xmlTag)

            For Each cnNode1 As System.Xml.XmlNode In list

                For Each cnNode2 As System.Xml.XmlNode In cnNode1.ChildNodes

                    Select Case cnNode2.Name
                        Case "Country_Code"
                            contactInfo.CountryCode = cnNode2.InnerText
                        Case "Area_Code"
                            contactInfo.AreaCode = cnNode2.InnerText
                        Case "Tel_No"
                            contactInfo.TelNo = cnNode2.InnerText
                        Case "Local_No"
                            contactInfo.LocalNo = cnNode2.InnerText
                    End Select
                Next

                'contactInfo. = xmlTag
            Next
        End Sub

#End Region

#Region " GetMemberMCRecord "

        Private pACCMCRecordClassResult As ACCMCRecordClassResult
        Public Property ACCMCRecordClassResult As ACCMCRecordClassResult
            Get
                Return pACCMCRecordClassResult
            End Get
            Set(ByVal value As ACCMCRecordClassResult)
                pACCMCRecordClassResult = value
            End Set
        End Property

        Public Function GetMemberMCRecord(ByVal mid As String, ByRef response As String) As Boolean
            Try
                'Dim client = New PagibigWebService.Service()
                'client.Url = API_LINK
                'Dim result = client.GetMemberMCRecord(USER_ID, Decrypt(USER_PASS), mid)
                'If result.IsGet Then
                '    PopulateMemberMCRecord(result)
                '    Return True
                'Else
                '    Return False
                '    response = result.GetErrorMsg()
                'End If
                Dim SoapResponse As String = ""
                Dim ErrMsg As String = ""
                If Execute_WSDL(API_LINK, SoapRequest("GetMemberMCRecord", New String() {"UID", "PWD", "PagIBIGID"}, New String() {USER_ID, Decrypt(USER_PASS), mid}), SoapResponse, ErrMsg) Then
                    '    If Execute_WSDL(API_LINK, SoapRequest("GetMemberMCRecord", New String() {"UID", "PWD", "PagIBIGID"}, New String() {USER_ID, USER_PASS, mid}), SoapResponse, ErrMsg) Then
                    PopulateMemberMCRecord(SoapResponse)
                    Return True
                Else
                    response = ErrMsg
                    Return False
                End If

            Catch ex As Exception
                response = ex.Message
                Return False
            Finally
            End Try
        End Function
        Private Sub PopulateMemberMCRecord(ByVal result As PagibigWebService.Result_MemberMCRec)

            pACCMCRecordClassResult = New ACCMCRecordClassResult
            pACCMCRecordClassResult.IsGet = result.IsGet
            pACCMCRecordClassResult.ErrorMessage = result.GetErrorMsg()
            pACCMCRecordClassResult.ACCMCRecordClass = PopulateHDMFMCRecord(result.MemberMcRec)

        End Sub
        Private Function PopulateHDMFMCRecord(ByVal result As PagibigWebService.HDMFMCRecord()) As List(Of MemberContribution)
            Dim contributions As New List(Of MemberContribution)

            For Each rec As PagibigWebService.HDMFMCRecord In result
                Dim mcRecord As New MemberContribution
                mcRecord.PagIBIGID = rec.PagIbigID
                mcRecord.IngresID = rec.IngresID
                mcRecord.MCLastName = rec.MemberLastName
                mcRecord.MCFirstName = rec.MemberFirstName
                mcRecord.MCMiddleName = rec.MemberMiddleName
                mcRecord.MCExt = rec.MemberNameExtension
                mcRecord.MCDateOfBirth = rec.DateOfBirth
                mcRecord.MCInitialPFRNumber = rec.InitialPFRNumber
                mcRecord.MCLastPFRDate = rec.InitialPFRDate
                mcRecord.MCInitialPFRAmount = rec.InitialPFRAmt
                mcRecord.MCLastPeriodCover = rec.LastPeriodCover
                mcRecord.MCLastPFRNumber = rec.LastPFRNumber
                mcRecord.MCLastPFRDate = rec.LastPFRDate
                mcRecord.MCLastPFRAmount = rec.LastPFRAmt
                mcRecord.MCTAVBalance = rec.TAVBalance
                mcRecord.MCEmployerBranch = rec.Branch
                mcRecord.MCEmployerName = rec.EmployerName
                mcRecord.MCStatus = rec.Status

                contributions.Add(mcRecord)
            Next

            Return contributions
        End Function


        Private Sub PopulateMemberMCRecord(ByVal responsebody As String)
            Dim x As System.Xml.XmlDocument = New System.Xml.XmlDocument()
            x.LoadXml(responsebody)

            pACCMCRecordClassResult = New ACCMCRecordClassResult

            Dim list As System.Xml.XmlNodeList = x.GetElementsByTagName("GetMemberMCRecordResult")

            For Each cnNode1 As System.Xml.XmlNode In list

                For Each cnNode2 As System.Xml.XmlNode In cnNode1.ChildNodes

                    Select Case cnNode2.Name
                        Case "IsGet"
                            pACCMCRecordClassResult.IsGet = Convert.ToBoolean(cnNode2.InnerText)
                        Case "MemberMcRec"
                            Dim ACCMCRecordClass As New List(Of MemberContribution)
                            Dim memberMCRec As New MemberContribution
                            PopulateHDMFMCRecord(responsebody, memberMCRec)
                            ACCMCRecordClass.Add(memberMCRec)
                            pACCMCRecordClassResult.ACCMCRecordClass = ACCMCRecordClass
                        Case "GetErrorMsg"
                            pACCMCRecordClassResult.ErrorMessage = cnNode2.InnerText
                    End Select
                Next
            Next
        End Sub
        Private Shared Sub PopulateHDMFMCRecord(ByVal responsebody As String, ByRef mcRecord As MemberContribution)
            Dim x As System.Xml.XmlDocument = New System.Xml.XmlDocument()
            x.LoadXml(responsebody)
            Dim list As System.Xml.XmlNodeList = x.GetElementsByTagName("HDMFMCRecord")

            For Each cnNode1 As System.Xml.XmlNode In list

                For Each cnNode2 As System.Xml.XmlNode In cnNode1.ChildNodes

                    Select Case cnNode2.Name
                        Case "PagIbigID"
                            mcRecord.PagIBIGID = cnNode2.InnerText
                        Case "IngresID"
                            mcRecord.IngresID = cnNode2.InnerText
                        Case "MemberLastName"
                            mcRecord.MCLastName = cnNode2.InnerText
                        Case "MemberFirstName"
                            mcRecord.MCFirstName = cnNode2.InnerText
                        Case "MemberMiddleName"
                            mcRecord.MCMiddleName = cnNode2.InnerText
                        Case "MemberNameExtension"
                            mcRecord.MCExt = cnNode2.InnerText
                        Case "DateOfBirth"
                            mcRecord.MCDateOfBirth = cnNode2.InnerText
                        Case "InitialPFRNumber"
                            mcRecord.MCInitialPFRNumber = cnNode2.InnerText
                        Case "InitialPFRDate"
                            mcRecord.MCLastPFRDate = cnNode2.InnerText
                        Case "InitialPFRAmt"
                            mcRecord.MCInitialPFRAmount = cnNode2.InnerText
                        Case "LastPeriodCover"
                            mcRecord.MCLastPeriodCover = cnNode2.InnerText
                        Case "LastPFRNumber"
                            mcRecord.MCLastPFRNumber = cnNode2.InnerText
                        Case "LastPFRDate"
                            mcRecord.MCLastPFRDate = cnNode2.InnerText
                        Case "LastPFRAmt"
                            mcRecord.MCLastPFRAmount = cnNode2.InnerText
                        Case "TAVBalance"
                            mcRecord.MCTAVBalance = cnNode2.InnerText
                        Case "Branch"
                            mcRecord.MCEmployerBranch = cnNode2.InnerText
                        Case "EmployerName"
                            mcRecord.MCEmployerName = cnNode2.InnerText
                        Case "Status"
                            mcRecord.MCStatus = cnNode2.InnerText
                    End Select
                Next
            Next
        End Sub

#End Region

#Region " PushCardInfo "

        Private pPushCardInfoResult As PushCardInfoResult
        Public Property PushCardInfoResult As PushCardInfoResult
            Get
                Return pPushCardInfoResult
            End Get
            Set(ByVal value As PushCardInfoResult)
                pPushCardInfoResult = value
            End Set
        End Property

        Public Function PushCardInfo(ByVal MID As String, ByVal Lname As String, ByVal Fname As String, ByVal Mname As String,
                                     ByVal Birthdate As Date, ByVal Mobileno As String, ByVal Cardno As String, ByVal AccountNo As String,
                                     ByVal ExpiryDate As Date, ByVal Dateissued As Date, ByVal BankCode As String, ByVal MsbCode As String,
                                     ByRef response As String) As Boolean
            Try

                'Dim client = New PagibigWebService.Service()
                'client.Url = API_LINK
                'Dim result = client.PushCardInfo(USER_ID, Decrypt(USER_PASS), MID, Lname, Fname, Mname, Birthdate, Mobileno, Cardno, AccountNo, ExpiryDate, Dateissued, BankCode, MsbCode)
                'If result.Is_success Then
                '    PopulatePushCardInfoResult(result)
                '    Return True
                'Else
                '    Return False
                '    response = result.GetErrorMsg()
                'End If

                Dim SoapResponse As String = ""
                Dim ErrMsg As String = ""
                If Execute_WSDL(API_LINK, SoapRequest("PushCardInfo",
                                                      New String() {"userid", "user_password",
                                                                    "pagibigid", "lname", "fname", "mname", "birthdate", "mobileno",
                                                                    "cardno", "accountno", "expiry_date", "date_issued", "bank_code", "msb_code"
                                                                   },
                                                      New String() {USER_ID, Decrypt(USER_PASS),
                                                                    MID, Lname, Fname, Mname, Birthdate, Mobileno,
                                                                    Cardno, AccountNo, ExpiryDate, Dateissued, BankCode, MsbCode
                                                                    }), SoapResponse, ErrMsg) Then
                    PopulatePushCardInfoResult(SoapResponse)
                    System.IO.File.WriteAllText(My.Settings.WS_Repo & "\Logs\PushCardInfo" & Now.ToString("hhmmss") & ".txt", SoapResponse)

                    Return True
                Else
                    response = ErrMsg
                    System.IO.File.WriteAllText(My.Settings.WS_Repo & "\Logs\PushCardInfo" & Now.ToString("hhmmss") & ".txt", response)
                    'Return FailResponse
                    Return False
                End If

            Catch ex As Exception
                response = ex.Message
                Return False
            Finally
            End Try
        End Function
        Private Sub PopulatePushCardInfoResult(ByVal result As PagibigWebService.ExecuteResult)
            pPushCardInfoResult = New PushCardInfoResult
            pPushCardInfoResult.IsSuccess = result.Is_success
            pPushCardInfoResult.GetErrorMsg = result.GetErrorMsg()
        End Sub
        Private Sub PopulatePushCardInfoResult(ByVal responsebody As String)
            Dim x As System.Xml.XmlDocument = New System.Xml.XmlDocument()
            x.LoadXml(responsebody)
            Dim list As System.Xml.XmlNodeList = x.GetElementsByTagName("PushCardInfoResult")

            pPushCardInfoResult = New PushCardInfoResult

            For Each cnNode1 As System.Xml.XmlNode In list

                For Each cnNode2 As System.Xml.XmlNode In cnNode1.ChildNodes

                    Select Case cnNode2.Name
                        Case "Is_success"
                            pPushCardInfoResult.IsSuccess = Convert.ToBoolean(cnNode2.InnerText)
                        Case "GetErrorMsg"
                            pPushCardInfoResult.GetErrorMsg = cnNode2.InnerText
                    End Select
                Next
            Next
        End Sub

#End Region

#Region " UpdateCard Info "

        Private pUpdateCardInfoResult As UpdateCardInfoResult
        Public Property UpdateCardInfoResult As UpdateCardInfoResult
            Get
                Return pUpdateCardInfoResult
            End Get
            Set(ByVal value As UpdateCardInfoResult)
                pUpdateCardInfoResult = value
            End Set
        End Property

        Public Function UpdateCardInfo(ByVal pagibigid As String, ByVal old_cardno As String, ByVal new_cardno As String, ByVal accountno As String,
                                     ByVal bank_code As String, ByRef response As String) As Boolean
            Try
                'Dim client = New PagibigWebService.Service()
                'client.Url = API_LINK
                'Dim result = client.UpdateCardInfo(USER_ID, Decrypt(USER_PASS), pagibigid, old_cardno, new_cardno, accountno, bank_code)
                'If result.Is_success Then
                '    PopulateUpdateCardInfoResult(result)
                '    Return True
                'Else
                '    Return False
                '    response = result.GetErrorMsg()
                'End If
                Dim SoapResponse As String = ""
                Dim ErrMsg As String = ""

                If Execute_WSDL(API_LINK, SoapRequest("UpdateCardInfo",
                                                      New String() {"userid", "user_password",
                                                                    "pagibigid", "old_cardno", "new_cardno", "accountno", "bank_code"
                                                                   },
                                                      New String() {USER_ID, Decrypt(USER_PASS),
                                                                    pagibigid, old_cardno, new_cardno, accountno, bank_code
                                                                    }), SoapResponse, ErrMsg) Then
                    PopulateUpdateCardInfoResult(SoapResponse)
                    System.IO.File.WriteAllText(My.Settings.WS_Repo & "\Logs\UpdateCardInfo" & Now.ToString("hhmmss") & ".txt", SoapResponse)

                    Return True
                Else
                    response = ErrMsg
                    System.IO.File.WriteAllText(My.Settings.WS_Repo & "\Logs\UpdateCardInfo" & Now.ToString("hhmmss") & ".txt", response)
                    'Return FailResponse
                    Return False
                End If

            Catch ex As Exception
                response = ex.Message
                Return False
            Finally
            End Try
        End Function
        Private Sub PopulateUpdateCardInfoResult(ByVal result As PagibigWebService.ExecuteResult)
            pUpdateCardInfoResult = New UpdateCardInfoResult
            pUpdateCardInfoResult.IsSuccess = result.Is_success
            pUpdateCardInfoResult.GetErrorMsg = result.GetErrorMsg()
        End Sub
        Private Sub PopulateUpdateCardInfoResult(ByVal responsebody As String)
            Dim x As System.Xml.XmlDocument = New System.Xml.XmlDocument()
            x.LoadXml(responsebody)
            Dim list As System.Xml.XmlNodeList = x.GetElementsByTagName("UpdateCardInfoResult")

            pUpdateCardInfoResult = New UpdateCardInfoResult

            For Each cnNode1 As System.Xml.XmlNode In list

                For Each cnNode2 As System.Xml.XmlNode In cnNode1.ChildNodes

                    Select Case cnNode2.Name
                        Case "Is_success"
                            pUpdateCardInfoResult.IsSuccess = Convert.ToBoolean(cnNode2.InnerText)
                        Case "GetErrorMsg"
                            pUpdateCardInfoResult.GetErrorMsg = cnNode2.InnerText
                    End Select
                Next
            Next
        End Sub

#End Region

#Region "ActiveCardInfo"
        Private pActiveCardInfoResult As ActiveCardInfoResult
        Public Property ActiveCardInfoResult As ActiveCardInfoResult
            Get
                Return pActiveCardInfoResult
            End Get
            Set(ByVal value As ActiveCardInfoResult)
                pActiveCardInfoResult = value
            End Set
        End Property

        Public Function ActiveCardInfo(ByVal pagibigid As String, ByVal bank_code As String, ByRef response As String) As Boolean
            Try
                'Dim client = New PagibigWebService.Service()
                'client.Url = API_LINK
                'Dim result = client.ActiveCardInfo(USER_ID, Decrypt(USER_PASS), pagibigid, bank_code)
                'If result.IsGet Then
                '    PopulateActiveCardInfoResult(result)
                '    Return True
                'Else
                '    Return False
                '    response = result.RequestError()
                'End If

                Dim SoapResponse As String = ""
                Dim ErrMsg As String = ""

                If Execute_WSDL(API_LINK, SoapRequest("ActiveCardInfo ",
                                                      New String() {"userid", "user_password",
                                                                    "pagibigid", "bank_code"
                                                                   },
                                                      New String() {USER_ID, Decrypt(USER_PASS),
                                                                    pagibigid, bank_code
                                                                    }), SoapResponse, ErrMsg) Then
                    PopulateActiveCardInfoResult(SoapResponse)
                    System.IO.File.WriteAllText(My.Settings.WS_Repo & "\Logs\ActiveCardInfo" & Now.ToString("hhmmss") & ".txt", SoapResponse)

                    Return True
                Else
                    response = ErrMsg
                    System.IO.File.WriteAllText(My.Settings.WS_Repo & "\Logs\ActiveCardInfo" & Now.ToString("hhmmss") & ".txt", response)
                    'Return FailResponse
                    Return False
                End If

            Catch ex As Exception
                response = ex.Message
                Return False
            Finally
            End Try
        End Function
        Private Sub PopulateActiveCardInfoResult(ByVal result As PagibigWebService.Card_ExecuteResult)
            pActiveCardInfoResult = New ActiveCardInfoResult
            pActiveCardInfoResult.IsGet = result.IsGet
            pActiveCardInfoResult.CardNumber = result.CardNumber
            pActiveCardInfoResult.ErrorMessage = result.RequestError

        End Sub

        Private Sub PopulateActiveCardInfoResult(ByVal responsebody As String)
            Dim x As System.Xml.XmlDocument = New System.Xml.XmlDocument()
            x.LoadXml(responsebody)
            Dim list As System.Xml.XmlNodeList = x.GetElementsByTagName("ActiveCardInfoResult")

            pActiveCardInfoResult = New ActiveCardInfoResult

            For Each cnNode1 As System.Xml.XmlNode In list

                For Each cnNode2 As System.Xml.XmlNode In cnNode1.ChildNodes

                    Select Case cnNode2.Name
                        Case "IsGet"
                            pActiveCardInfoResult.IsGet = Convert.ToBoolean(cnNode2.InnerText)
                        Case "CardNumber"
                            pActiveCardInfoResult.CardNumber = cnNode2.InnerText
                        Case "RequestError"
                            pActiveCardInfoResult.ErrorMessage = cnNode2.InnerText
                    End Select
                Next
            Next
        End Sub
#End Region



#Region " AUB "

        Public Function SendAUBRequest(ByVal RequestType As DataKeysEnum.AUBRequest, ByVal aubRequest As AUBRequest, ByRef response As String) As Boolean
            Try
                Dim AUB_API_LINK = My.Settings.AUB_BasedURL

                Select Case RequestType
                    Case DataKeysEnum.AUBRequest.CreateAccount
                        AUB_API_LINK &= "/account/create"
                    Case DataKeysEnum.AUBRequest.ReplaceCard
                        AUB_API_LINK &= "/card/replace"
                    Case DataKeysEnum.AUBRequest.GetCardNo
                        AUB_API_LINK &= "/inquiry/get-cardno"
                End Select

                Dim SoapResponse As String = ""
                Dim ErrMsg As String = ""

                If Execute_WSDL(AUB_API_LINK, Newtonsoft.Json.JsonConvert.SerializeObject(aubRequest), SoapResponse, ErrMsg, "application/json; charset=utf-8") Then
                    response = SoapResponse
                    Return True
                Else
                    response = ErrMsg
                    'Return FailResponse
                    Return False
                End If

            Catch ex As Exception
                response = ex.Message
                Return False
            Finally
            End Try
        End Function

#End Region

        Public Shared Function Execute_WSDL(ByVal EndPoint As String, ByVal SoapMessage As String, ByRef SoapResponse As String, ByRef ErrMsg As String,
                                            Optional contentType As String = "text/xml; charset=utf-8") As Boolean
            Dim Request As System.Net.WebRequest
            Dim Response As System.Net.WebResponse
            Dim DataStream As System.IO.Stream
            Dim Reader As System.IO.StreamReader
            Dim SoapByte As Byte()
            Dim SoapStr As String = SoapMessage
            Dim pSuccess As Boolean = True

            Try

                'ServicePointManager.ServerCertificateValidationCallback =
                '     Function(se As Object,
                '     cert As X509Certificate,
                '     chain As X509Chain,
                '     sslerror As SslPolicyErrors) True

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

                SoapByte = System.Text.Encoding.UTF8.GetBytes(SoapStr)
                Request = System.Net.WebRequest.Create(EndPoint)
                'Request.ContentType = "text/xml; charset=utf-8"
                Request.ContentType = contentType
                Request.ContentLength = SoapByte.Length
                Request.Method = "POST"
                Request.Timeout = My.Settings.AUB_WS_TIMEOUT * 1000


                DataStream = Request.GetRequestStream()
                DataStream.Write(SoapByte, 0, SoapByte.Length)
                DataStream.Close()

                Response = Request.GetResponse()
                DataStream = Response.GetResponseStream()
                Reader = New System.IO.StreamReader(DataStream)
                Dim SD2Request As String = Reader.ReadToEnd()
                DataStream.Close()
                Reader.Close()
                Response.Close()
                SoapResponse = SD2Request


                'ServicePointManager.ServerCertificateValidationCallback = Nothing
                Return True
            Catch TimeoutEx As TimeoutException
                ErrMsg = TimeoutEx.Message
                Return False
            Catch ex As Exception
                ErrMsg = ex.Message
                Return False
            End Try
        End Function

        Private Shared Function SoapRequest(ByVal WebMethod As String, ByVal reqParams As String(), ByVal reqValues As String()) As String
            Dim SoapStr As System.Text.StringBuilder = New System.Text.StringBuilder()
            SoapStr.Append("<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">")
            SoapStr.Append("<soap:Body>")
            SoapStr.Append("<" & WebMethod & " xmlns=""http://tempuri.org/"">")

            For i As Integer = 0 To reqParams.Length - 1
                SoapStr.Append(String.Format("<{0}>{1}</{0}>", reqParams(i), reqValues(i)))
            Next

            SoapStr.Append("</" & WebMethod & ">")
            SoapStr.Append("</soap:Body>")
            SoapStr.Append("</soap:Envelope>")

            System.IO.File.WriteAllText(My.Settings.WS_Repo & "\Logs\SoapRequest" & Now.ToString("hhmmss") & ".txt", SoapStr.ToString)

            Return SoapStr.ToString()
        End Function

        Public Shared Function ToHubSpotTimestamp(ByVal target As DateTime) As Long
            Dim date1 As DateTime = New DateTime(1970, 1, 1, 0, 0, 0, target.Kind)
            Dim hubspotTimestamp As Long = System.Convert.ToInt64((target - date1).TotalSeconds)
            Return hubspotTimestamp ' * 1000
        End Function

        Public Shared Function GetAddressCodes(ByVal haCity As String, ByVal haProvince As String,
                                               ByVal paCity As String, ByVal paProvince As String) As String
            Dim DAL As New DAL
            Dim homeAddress_cityCode As String = ""
            Dim homeAddress_provinceCode As String = ""
            Dim permanentAddress_cityCode As String = ""
            Dim permanentAddress_provinceCode As String = ""

            If haCity = "" Then
            ElseIf haProvince = "" Then
            Else
                If DAL.SelectQuery(String.Format("SELECT ISNULL(cityCode,''),ISNULL(provinceCode,'') FROM tbl_AUB_AddressCode WHERE city='{0}' AND province='{1}'", haCity, haProvince)) Then
                    If DAL.TableResult.DefaultView.Count > 0 Then
                        Dim rw As DataRow = DAL.TableResult.Rows(0)
                        If Not IsDBNull(rw(0)) Then homeAddress_cityCode = rw(0).ToString.Trim
                        If Not IsDBNull(rw(0)) Then homeAddress_provinceCode = rw(1).ToString.Trim
                    End If
                End If
            End If

            If paCity = "" Then
            ElseIf paProvince = "" Then
            Else
                If DAL.SelectQuery(String.Format("SELECT ISNULL(cityCode,''),ISNULL(provinceCode,'') FROM tbl_AUB_AddressCode WHERE city='{0}' AND province='{1}'", paCity, paProvince)) Then
                    If DAL.TableResult.DefaultView.Count > 0 Then
                        Dim rw As DataRow = DAL.TableResult.Rows(0)
                        If Not IsDBNull(rw(0)) Then permanentAddress_cityCode = rw(0).ToString.Trim
                        If Not IsDBNull(rw(0)) Then permanentAddress_provinceCode = rw(1).ToString.Trim
                    End If
                End If
            End If

            DAL.Dispose()
            DAL = Nothing

            Return String.Format("{0}|{1}|{2}|{3}", homeAddress_cityCode, homeAddress_provinceCode, permanentAddress_cityCode, permanentAddress_provinceCode)
        End Function

        Public Shared Function GetAUBIDType(ByVal IDType As String) As String
            Dim DAL As New DAL
            Dim aubIDType As String = ""
            If IDType = "" Then
            Else
                If DAL.ExecuteScalar("SELECT ISNULL(IDCode,'') FROM tbl_AUB_IDType WHERE IDTypeID=" & IDType) Then
                    If Not IsDBNull(DAL.ObjectResult) Then aubIDType = DAL.ObjectResult.ToString.Trim
                End If
            End If

            DAL.Dispose()
            DAL = Nothing

            Return aubIDType
        End Function

        Public Shared Function Encrypt(ByVal clearText As String) As String
            Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
            Using encryptor As System.Security.Cryptography.Aes = System.Security.Cryptography.Aes.Create()
                Dim pdb As New System.Security.Cryptography.Rfc2898DeriveBytes(encryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
             &H65, &H64, &H76, &H65, &H64, &H65,
             &H76})
                encryptor.Key = pdb.GetBytes(32)
                encryptor.IV = pdb.GetBytes(16)
                Using ms As New System.IO.MemoryStream()
                    Using cs As New System.Security.Cryptography.CryptoStream(ms, encryptor.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write)
                        cs.Write(clearBytes, 0, clearBytes.Length)
                        cs.Close()
                    End Using
                    clearText = Convert.ToBase64String(ms.ToArray())
                End Using
            End Using
            Return clearText
        End Function

        Public Shared Function Decrypt(ByVal cipherText As String) As String
            Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
            Using decryptor As System.Security.Cryptography.Aes = System.Security.Cryptography.Aes.Create()
                Dim pdb As New System.Security.Cryptography.Rfc2898DeriveBytes(encryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
             &H65, &H64, &H76, &H65, &H64, &H65,
             &H76})
                decryptor.Key = pdb.GetBytes(32)
                decryptor.IV = pdb.GetBytes(16)
                Using ms As New System.IO.MemoryStream()
                    Using cs As New System.Security.Cryptography.CryptoStream(ms, decryptor.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write)
                        cs.Write(cipherBytes, 0, cipherBytes.Length)
                        cs.Close()
                    End Using
                    cipherText = Encoding.Unicode.GetString(ms.ToArray())
                End Using
            End Using
            Return cipherText
        End Function

        Public Shared Sub PackUBPData(ByVal RequestAuth As RequestAuth,
                                      ByVal arrData As String,
                                      ByVal MID As String, ByVal AcctNo As String,
                                      ByVal Photo As Byte(), ByVal Signature As Byte(), ByVal PhotoID As Byte(),
                                      ByVal LP_ANSI As Byte(), ByVal LB_ANSI As Byte(), ByVal RP_ANSI As Byte(), ByVal RB_ANSI As Byte(),
                                      ByVal LP_WSQ As Byte(), ByVal LB_WSQ As Byte(), ByVal RP_WSQ As Byte(), ByVal RB_WSQ As Byte(), ByVal isRecard As Boolean)
            Try

                Dim repo As String = My.Settings.WS_Repo & "\UBP"
                If Not System.IO.Directory.Exists(repo) Then System.IO.Directory.CreateDirectory(repo)


                Dim fileName = AcctNo

                If isRecard Then
                    fileName = MID
                    ' Recard Format.
                    repo = My.Settings.WS_Repo & "\UBP\RECARD\" & fileName
                    If Not System.IO.Directory.Exists(repo) Then System.IO.Directory.CreateDirectory(repo)
                Else
                    repo = My.Settings.WS_Repo & "\UBP\" & fileName
                    If Not System.IO.Directory.Exists(repo) Then System.IO.Directory.CreateDirectory(repo)
                End If

                System.IO.File.WriteAllText(String.Format("{0}\{1}.txt", repo, fileName), arrData)

                If Not Photo Is Nothing Then
                    System.IO.File.WriteAllBytes(String.Format("{0}\{1}ph.jpg", repo, fileName), Photo)
                End If

                If Not Signature Is Nothing Then
                    System.IO.File.WriteAllBytes(String.Format("{0}\{1}s.jpg", repo, fileName), Signature)
                End If

                If Not PhotoID Is Nothing Then
                    System.IO.File.WriteAllBytes(String.Format("{0}\{1}i.jpg", repo, fileName), PhotoID)
                End If

                If Not LP_ANSI Is Nothing Then
                    System.IO.File.WriteAllBytes(String.Format("{0}\{1}FPLP-ansi.ansi", repo, fileName), LP_ANSI)
                End If

                If Not LB_ANSI Is Nothing Then
                    System.IO.File.WriteAllBytes(String.Format("{0}\{1}FPLB-ansi.ansi", repo, fileName), LB_ANSI)
                End If

                If Not RP_ANSI Is Nothing Then
                    System.IO.File.WriteAllBytes(String.Format("{0}\{1}FPRP-ansi.ansi", repo, fileName), RP_ANSI)
                End If

                If Not RB_ANSI Is Nothing Then
                    System.IO.File.WriteAllBytes(String.Format("{0}\{1}FPRB-ansi.ansi", repo, fileName), RB_ANSI)
                End If

                If Not LP_WSQ Is Nothing Then
                    System.IO.File.WriteAllBytes(String.Format("{0}\{1}FPLP-wsq.wsq", repo, fileName), LP_WSQ)
                End If

                If Not LB_WSQ Is Nothing Then
                    System.IO.File.WriteAllBytes(String.Format("{0}\{1}FPLB-wsq.wsq", repo, fileName), LB_WSQ)
                End If

                If Not RP_WSQ Is Nothing Then
                    System.IO.File.WriteAllBytes(String.Format("{0}\{1}FPRP-wsq.wsq", repo, fileName), RP_WSQ)
                End If

                If Not RB_WSQ Is Nothing Then
                    System.IO.File.WriteAllBytes(String.Format("{0}\{1}FPRB-wsq.wsq", repo, fileName), RB_WSQ)
                End If



            Catch ex As Exception
                PHASE3.SaveToErrorLog(RequestAuth, "PackUBPData(): Runtime error " & ex.Message)
                Throw ex
            End Try
        End Sub

        Public Shared Sub PackUBPData2(ByVal RequestAuth As RequestAuth,
                                      ByVal MID As String,
                                      ByVal Photo As Byte(), ByVal Signature As Byte(), ByVal PhotoID As Byte(),
                                      ByVal LP_ANSI As Byte(), ByVal LB_ANSI As Byte(), ByVal RP_ANSI As Byte(), ByVal RB_ANSI As Byte(),
                                      ByVal LP_WSQ As Byte(), ByVal LB_WSQ As Byte(), ByVal RP_WSQ As Byte(), ByVal RB_WSQ As Byte(), ByVal applicationDate As DateTime)
            Try
                Dim repo As String = My.Settings.WS_Repo & "\AUB\PACKUPDATA"
                If Not System.IO.Directory.Exists(repo) Then System.IO.Directory.CreateDirectory(repo)

                repo = My.Settings.WS_Repo & "\AUB\PACKUPDATA\" & MID
                If Not System.IO.Directory.Exists(repo) Then System.IO.Directory.CreateDirectory(repo)


                'System.IO.File.WriteAllText(String.Format("{0}\{1}.txt", repo, AcctNo), arrData)
                'arrData = arrData.Remove("\n")
                'System.IO.File.WriteAllText(String.Format("{0}\{1}.txt", repo, MID), arrData)

                'Dim sw As New System.IO.StreamWriter(fileNaming, True)
                'sw.WriteLine(arrData)
                'sw.Close()

                If Not Photo Is Nothing Then
                    'Dim photoImage = Utilities.byteArrayToImage(Photo)
                    'photoImage.Save(String.Format("{0}\{1}ph_{2}.jpg", repo, MID, applicationDate.ToString("yyyyMMdd")))

                    Dim ms As New IO.MemoryStream(Photo) 'This is correct...
                    Dim pic As System.Drawing.Image = System.Drawing.Image.FromStream(ms)
                    Dim SaveImage As New System.Drawing.Bitmap(pic)
                    SaveImage.Save(String.Format("{0}\{1}_{2}_ph.jpg", repo, MID, applicationDate.ToString("yyyyMMdd")), System.Drawing.Imaging.ImageFormat.Jpeg)
                    SaveImage.Dispose()
                    'System.IO.File.WriteAllBytes(String.Format("{0}\{1}_{2}_ph.jpg", repo, MID, applicationDate.ToString("yyyyMMdd")), Photo)
                End If

                If Not Signature Is Nothing Then
                    Dim ms As New IO.MemoryStream(Signature) 'This is correct...
                    Dim pic As System.Drawing.Image = System.Drawing.Image.FromStream(ms)
                    Dim SaveImage As New System.Drawing.Bitmap(pic)
                    SaveImage.Save(String.Format("{0}\{1}_{2}_s.jpg", repo, MID, applicationDate.ToString("yyyyMMdd")), System.Drawing.Imaging.ImageFormat.Jpeg)
                    SaveImage.Dispose()
                    'Dim signaturImage = Utilities.byteArrayToImage(Signature)
                    'signaturImage.Save(String.Format("{0}\{1}_{2}_s.jpg", repo, MID, applicationDate.ToString("yyyyMMdd")))

                End If

                If Not PhotoID Is Nothing Then
                    'Dim photoIDImage = Utilities.byteArrayToImage(PhotoID)
                    'photoIDImage.Save(String.Format("{0}\{1}i_{2}.jpg", repo, MID, applicationDate.ToString("yyyyMMdd")))
                    System.IO.File.WriteAllBytes(String.Format("{0}\{1}_{2}_i.jpg", repo, MID, applicationDate.ToString("yyyyMMdd")), PhotoID)
                End If

                If Not LP_ANSI Is Nothing Then
                    System.IO.File.WriteAllBytes(String.Format("{0}\{1}_{2}_FPLP-ansi.ansi", repo, MID, applicationDate.ToString("yyyyMMdd")), LP_ANSI)
                End If

                If Not LB_ANSI Is Nothing Then
                    System.IO.File.WriteAllBytes(String.Format("{0}\{1}_{2}_FPLB-ansi.ansi", repo, MID, applicationDate.ToString("yyyyMMdd")), LB_ANSI)
                End If

                If Not RP_ANSI Is Nothing Then
                    System.IO.File.WriteAllBytes(String.Format("{0}\{1}_{2}_FPRP-ansi.ansi", repo, MID, applicationDate.ToString("yyyyMMdd")), RP_ANSI)
                End If

                If Not RB_ANSI Is Nothing Then
                    System.IO.File.WriteAllBytes(String.Format("{0}\{1}_{2}_FPRB-ansi.ansi", repo, MID, applicationDate.ToString("yyyyMMdd")), RB_ANSI)
                End If

                If Not LP_WSQ Is Nothing Then
                    System.IO.File.WriteAllBytes(String.Format("{0}\{1}_{2}_FPLP-wsq.wsq", repo, MID, applicationDate.ToString("yyyyMMdd")), LP_WSQ)
                End If

                If Not LB_WSQ Is Nothing Then
                    System.IO.File.WriteAllBytes(String.Format("{0}\{1}_{2}_FPLB-wsq.wsq", repo, MID, applicationDate.ToString("yyyyMMdd")), LB_WSQ)
                End If

                If Not RP_WSQ Is Nothing Then
                    System.IO.File.WriteAllBytes(String.Format("{0}\{1}_{2}_FPRP-wsq.wsq", repo, MID, applicationDate.ToString("yyyyMMdd")), RP_WSQ)
                End If

                If Not RB_WSQ Is Nothing Then
                    System.IO.File.WriteAllBytes(String.Format("{0}\{1}_{2}_FPRB-wsq.wsq", repo, MID, applicationDate.ToString("yyyyMMdd")), RB_WSQ)
                End If


            Catch ex As Exception
                PHASE3.SaveToErrorLog(RequestAuth, "PackUBPData2(): Runtime error " & ex.Message)
                Throw ex
            End Try
        End Sub

#Region " Logs "

        Public Shared LogsRepo As String = My.Settings.WS_Repo & "\Logs"
        Public Shared SystemLog As String = "SystemLog.txt"
        Public Shared ErrorLog As String = "ErrorLog.txt"

        Public Shared Sub SaveToSystemLog(ByVal requestAuth As RequestAuth, ByVal data As String)
            Dim logRepo As String = String.Format("{0}\{1}", LogsRepo, Now.ToString("yyyy-MM-dd"))
            Dim fileLog As String = String.Format("{0}\{1}", logRepo, SystemLog)
            If Not System.IO.Directory.Exists(logRepo) Then System.IO.Directory.CreateDirectory(logRepo)

            Dim user As String = ""
            Dim kioskID As String = ""

            If Not requestAuth Is Nothing Then
                user = requestAuth.User
                kioskID = requestAuth.KioskID
            End If

            Dim message = String.Format("{0}|{1}|{2}|{3}", Now.ToString("MM/dd/yyyy hh:mm:ss tt"), requestAuth.User, requestAuth.KioskID, data) & vbNewLine
            File.AppendAllText(fileLog, message)
            'Using sw As New System.IO.StreamWriter(fileLog, True)
            '    sw.Write(String.Format("{0}|{1}|{2}|{3}", Now.ToString("MM/dd/yyyy hh:mm:ss tt"), requestAuth.User, requestAuth.KioskID, data) & vbNewLine)
            '    sw.Dispose()
            '    sw.Close()
            'End Using
        End Sub

        Public Shared Sub SaveToErrorLog(ByVal requestAuth As RequestAuth, ByVal data As String)
            Dim logRepo As String = String.Format("{0}\{1}", LogsRepo, Now.ToString("yyyy-MM-dd"))
            Dim fileLog As String = String.Format("{0}\{1}", logRepo, ErrorLog)
            If Not System.IO.Directory.Exists(logRepo) Then System.IO.Directory.CreateDirectory(logRepo)

            Dim user As String = ""
            Dim kioskID As String = ""

            If Not requestAuth Is Nothing Then
                user = requestAuth.User
                kioskID = requestAuth.KioskID
            End If

            Dim message = String.Format("{0}|{1}|{2}|{3}", Now.ToString("MM/dd/yyyy hh:mm:ss tt"), user, kioskID, data) & vbNewLine
            File.AppendAllText(fileLog, message)
            'Using sw As New System.IO.StreamWriter(fileLog, True)
            '    sw.Write(String.Format("{0}|{1}|{2}|{3}", Now.ToString("MM/dd/yyyy hh:mm:ss tt"), user, kioskID, data) & vbNewLine)
            '    sw.Dispose()
            '    sw.Close()
            'End Using
        End Sub

#End Region

        Private pLogEmployeeResult As LogEmployeeResult
        Public Property LogEmployeeResult As LogEmployeeResult
            Get
                Return pLogEmployeeResult
            End Get
            Set(ByVal value As LogEmployeeResult)
                pLogEmployeeResult = value
            End Set
        End Property

        Public Function LogEmployee(ByVal employeeNumber As String, ByVal picture As Byte(), ByVal kioskId As String, ByRef response As String) As Boolean
            Try
                Dim SoapResponse As String = ""
                Dim ErrMsg As String = ""

                If Execute_WSDL(TIME_KEEPING_LINK, SoapRequest("LogEmployee", New String() {"EmployeeNumber", "Picture", "KioskID"}, New String() {employeeNumber, Convert.ToBase64String(picture), kioskId}), SoapResponse, ErrMsg) Then
                    PopulateLogEmployee(SoapResponse)
                    Return True
                Else
                    response = ErrMsg
                    'Return FailResponse
                    Return False
                End If

            Catch ex As Exception
                response = ex.Message
                Return False
            Finally
            End Try
        End Function

        Private Sub PopulateLogEmployee(ByVal responsebody As String)
            Dim x As System.Xml.XmlDocument = New System.Xml.XmlDocument()
            x.LoadXml(responsebody)
            Dim list As System.Xml.XmlNodeList = x.GetElementsByTagName("LogEmployeeResult")

            pLogEmployeeResult = New LogEmployeeResult

            For Each cnNode1 As System.Xml.XmlNode In list

                For Each cnNode2 As System.Xml.XmlNode In cnNode1.ChildNodes

                    Select Case cnNode2.Name
                        Case "Status"
                            pLogEmployeeResult.Status = cnNode2.InnerText
                        Case "Description"
                            pLogEmployeeResult.Description = cnNode2.InnerText
                        Case "DateTime"
                            pLogEmployeeResult.DateTime = cnNode2.InnerText
                    End Select
                Next
            Next
        End Sub

    End Class

End Namespace
