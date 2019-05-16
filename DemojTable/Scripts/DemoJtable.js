/// <reference path="jtable/jquery.jtable.js" />
    $(document).ready(function () {
        $('#PersonTableContainer').jtable({
            title: 'Customers',
            paging: true,
            pageSize: 10,
            sorting: true,
            actions: {
                listAction: '/Demo/GetCustomers'
         
            },
            fields: {
                CustomerID: {
                    key: true,
                    list: false
                },
                CompanyName: {
                    title: 'Company Name',
                    width: '40%'
                },
                ContactName: {
                    title: 'ContactName',
                    width: '20%'
                },
                ContactTitle: {
                    title: 'ContactTitle',
                    width: '20%'
                },
                City: {
                    title: 'City',
                    width: '20%'
                },
                PostalCode: {
                    title: 'PostalCode',
                    width: '20%'
                },
                Country: {
                    title: 'Country',
                    width: '20%'
                },
                Phone: {
                    title: 'Phone',
                    width: '20%'
                }
            },
            //Initialize validation logic when a form is created
            formCreated: function (event, data) {
                data.form.find('input[name="CompanyName"]').addClass('validate[required]');
                data.form.find('input[name="ContactName"]').addClass('validate[required]');
                data.form.find('input[name="ContactTitle"]').addClass('validate[required]');
                data.form.find('input[name="City"]').addClass('validate[required]');
                data.form.find('input[name="PostalCode"]').addClass('validate[required]');
                data.form.find('input[name="Country"]').addClass('validate[required]');
                data.form.find('input[name="Phone"]').addClass('validate[required]');
                data.form.validationEngine();
            },
            //Validate form when it is being submitted
            formSubmitting: function (event, data) {
                return data.form.validationEngine('validate');
            },
            //Dispose validation logic when form is closed
            formClosed: function (event, data) {
                data.form.validationEngine('hide');
                data.form.validationEngine('detach');
            }
        });

        //Re-load records when user click 'load records' button.
        $('#BtnSearch').click(function (e) {
            e.preventDefault();
            $('#PersonTableContainer').jtable('load', {
                companyname: $('#companyname').val()
            });
        });
 
        //Load all records when page is first shown
        $('#BtnSearch').click();

        $('#PersonTableContainer').jtable('load');
    });
