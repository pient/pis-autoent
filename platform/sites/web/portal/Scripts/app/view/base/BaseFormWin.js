Ext.define('PIS.view.base.BaseFormWin', {
    extend: 'PIS.FormWin',
    title: '表单',
    form: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
        }, config);

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        me.addEvents(
            'save',
            'cancel'
        );

        this.callParent(arguments);
    },

    onSaveClick: function () {
        var me = this;

        var _win = me.getFormWin();

        var _rec = _win.getRecord();

        _win.fireEvent('save', _win, _win.action, _rec, me);
    },

    doHide: function () {
        var me = this;

        var _win = me.up("window");

        _win.reset();
        _win.hide();
    },

    getFormWin: function () {
        var _win = me;

        if (!(me instanceof PIS.view.base.BaseFormWin)) {
            _win = me.up("window");
        }

        return _win;
    }
});