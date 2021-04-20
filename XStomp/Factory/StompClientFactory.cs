/*
 * https://github.com/krlito/StompNet
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace XStomp.Factory
{
    class StompClientFactory
    {
        //public static StompFrame CreateConnect(string acceptVersion, string host = null, string login = null, string passcode = null, Heartbeat heartbeat = null, IEnumerable<KeyValuePair<string, string>> extraHeaders = null)
        //{
        //    // This should be commented when multiversion is implemented.
        //    if (host == null)
        //        throw new ArgumentNullException("host");

        //    var headers = new List<KeyValuePair<string, string>>(5);
        //    headers.Add(new KeyValuePair<string, string>(StompHeaders.AcceptVersion, acceptVersion));
        //    /*if (host != null)*/
        //    headers.Add(new KeyValuePair<string, string>(StompHeaders.Host, host));
        //    if (login != null) headers.Add(new KeyValuePair<string, string>(StompHeaders.Login, login));
        //    if (passcode != null) headers.Add(new KeyValuePair<string, string>(StompHeaders.Passcode, passcode));
        //    if (heartbeat != null) headers.Add(new KeyValuePair<string, string>(StompHeaders.Heartbeat, heartbeat.RawHeartbeat));

        //    return new StompFrame(StompCommands.Connect, extraHeaders == null ? headers : headers.Concat(extraHeaders));
        //}

        //public static StompFrame CreateSend(
        //    string destination,
        //    byte[] body = null,
        //    string contentType = MediaTypeNames.Application.Octet,
        //    string receipt = null,
        //    string transaction = null,
        //    IEnumerable<KeyValuePair<string, string>> extraHeaders = null)
        //{
        //    if (destination == null)
        //        throw new ArgumentNullException("destination");

        //    var headers = new List<KeyValuePair<string, string>>(5);
        //    headers.Add(new KeyValuePair<string, string>(StompHeaders.Destination, destination));
        //    if (receipt != null) headers.Add(new KeyValuePair<string, string>(StompHeaders.Receipt, receipt));
        //    if (contentType != null) headers.Add(new KeyValuePair<string, string>(StompHeaders.ContentType, contentType));
        //    if (transaction != null) headers.Add(new KeyValuePair<string, string>(StompHeaders.Transaction, transaction));
        //    headers.Add(new KeyValuePair<string, string>(StompHeaders.ContentLength, body != null ? body.Length.ToString() : "0"));

        //    return new StompFrame(StompCommands.Send, extraHeaders == null ? headers : headers.Concat(extraHeaders), body);
        //}

        //public static StompFrame CreateSend(
        //    string destination,
        //    string body,
        //    Encoding encoding,
        //    string receipt = null,
        //    string transaction = null,
        //    IEnumerable<KeyValuePair<string, string>> extraHeaders = null)
        //{
        //    return CreateSend(destination,
        //        encoding.GetBytes(body),
        //        MediaTypeNames.Text.Plain + ";charset=" + encoding.WebName,
        //        receipt,
        //        transaction,
        //        extraHeaders);
        //}

        //public static StompFrame CreateSend(
        //    string destination,
        //    string body,
        //    string receipt = null,
        //    string transaction = null,
        //    IEnumerable<KeyValuePair<string, string>> extraHeaders = null)
        //{
        //    return CreateSend(destination,
        //        body,
        //        Encoding.UTF8,
        //        receipt,
        //        transaction,
        //        extraHeaders);
        //}

        //public static StompFrame CreateSubscribe(
        //    string destination,
        //    string id,
        //    string receipt = null,
        //    string ack = null,
        //    IEnumerable<KeyValuePair<string, string>> extraHeaders = null)
        //{
        //    if (destination == null)
        //        throw new ArgumentNullException("destination");

        //    if (id == null)
        //        throw new ArgumentNullException("id");

        //    if (ack != null)
        //        StompAckValues.ThrowIfInvalidAckValue(ack);

        //    var headers = new List<KeyValuePair<string, string>>(4);
        //    headers.Add(new KeyValuePair<string, string>(StompHeaders.Destination, destination));
        //    headers.Add(new KeyValuePair<string, string>(StompHeaders.Id, id));
        //    if (receipt != null) headers.Add(new KeyValuePair<string, string>(StompHeaders.Receipt, receipt));
        //    if (ack != null) headers.Add(new KeyValuePair<string, string>(StompHeaders.Ack, ack));

        //    return new StompFrame(StompCommands.Subscribe, extraHeaders == null ? headers : headers.Concat(extraHeaders));
        //}

        //public static StompFrame CreateUnsubscribe(
        //    string id,
        //    string receipt = null,
        //    IEnumerable<KeyValuePair<string, string>> extraHeaders = null)
        //{

        //    if (id == null)
        //        throw new ArgumentNullException("id");

        //    var headers = new List<KeyValuePair<string, string>>(2);
        //    headers.Add(new KeyValuePair<string, string>(StompHeaders.Id, id));
        //    if (receipt != null) headers.Add(new KeyValuePair<string, string>(StompHeaders.Receipt, receipt));

        //    return new StompFrame(StompCommands.Unsubscribe, extraHeaders == null ? headers : headers.Concat(extraHeaders));
        //}

        //public static StompFrame CreateAck(
        //    string id,
        //    string receipt = null,
        //    string transaction = null,
        //    IEnumerable<KeyValuePair<string, string>> extraHeaders = null)
        //{
        //    if (id == null)
        //        throw new ArgumentNullException("id");

        //    var headers = new List<KeyValuePair<string, string>>(3);
        //    headers.Add(new KeyValuePair<string, string>(StompHeaders.Id, id));
        //    if (receipt != null) headers.Add(new KeyValuePair<string, string>(StompHeaders.Receipt, receipt));
        //    if (transaction != null) headers.Add(new KeyValuePair<string, string>(StompHeaders.Transaction, transaction));

        //    return new StompFrame(StompCommands.Ack, extraHeaders == null ? headers : headers.Concat(extraHeaders));
        //}

        //public static StompFrame CreateNack(
        //    string id,
        //    string receipt = null,
        //    string transaction = null,
        //    IEnumerable<KeyValuePair<string, string>> extraHeaders = null)
        //{
        //    if (id == null)
        //        throw new ArgumentNullException("id");

        //    var headers = new List<KeyValuePair<string, string>>(3);
        //    headers.Add(new KeyValuePair<string, string>(StompHeaders.Id, id));
        //    if (receipt != null) headers.Add(new KeyValuePair<string, string>(StompHeaders.Receipt, receipt));
        //    if (transaction != null) headers.Add(new KeyValuePair<string, string>(StompHeaders.Transaction, transaction));

        //    return new StompFrame(StompCommands.Nack, extraHeaders == null ? headers : headers.Concat(extraHeaders));
        //}

        //public static StompFrame CreateBegin(
        //    string transaction,
        //    string receipt = null,
        //    IEnumerable<KeyValuePair<string, string>> extraHeaders = null)
        //{
        //    if (transaction == null)
        //        throw new ArgumentNullException("transaction");

        //    var headers = new List<KeyValuePair<string, string>>(2);
        //    headers.Add(new KeyValuePair<string, string>(StompHeaders.Transaction, transaction));
        //    if (receipt != null) headers.Add(new KeyValuePair<string, string>(StompHeaders.Receipt, receipt));

        //    return new StompFrame(StompCommands.Begin, extraHeaders == null ? headers : headers.Concat(extraHeaders));
        //}

        //public static StompFrame CreateCommit(
        //    string transaction,
        //    string receipt = null,
        //    IEnumerable<KeyValuePair<string, string>> extraHeaders = null)
        //{
        //    if (transaction == null)
        //        throw new ArgumentNullException("transaction");

        //    var headers = new List<KeyValuePair<string, string>>(2);
        //    headers.Add(new KeyValuePair<string, string>(StompHeaders.Transaction, transaction));
        //    if (receipt != null) headers.Add(new KeyValuePair<string, string>(StompHeaders.Receipt, receipt));

        //    return new StompFrame(StompCommands.Commit, extraHeaders == null ? headers : headers.Concat(extraHeaders));
        //}

        //public static StompFrame CreateAbort(
        //    string transaction,
        //    string receipt = null,
        //    IEnumerable<KeyValuePair<string, string>> extraHeaders = null)
        //{
        //    if (transaction == null)
        //        throw new ArgumentNullException("transaction");

        //    var headers = new List<KeyValuePair<string, string>>(2);
        //    headers.Add(new KeyValuePair<string, string>(StompHeaders.Transaction, transaction));
        //    if (receipt != null) headers.Add(new KeyValuePair<string, string>(StompHeaders.Receipt, receipt));

        //    return new StompFrame(StompCommands.Abort, extraHeaders == null ? headers : headers.Concat(extraHeaders));
        //}

        //public static StompFrame CreateDisconnect(
        //    string receipt = null,
        //    IEnumerable<KeyValuePair<string, string>> extraHeaders = null)
        //{

        //    var headers = new List<KeyValuePair<string, string>>(1);
        //    if (receipt != null) headers.Add(new KeyValuePair<string, string>(StompHeaders.Receipt, receipt));

        //    return new StompFrame(StompCommands.Disconnect, extraHeaders == null ? headers : headers.Concat(extraHeaders));
        //}
    }
}
